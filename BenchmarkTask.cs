using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
    {
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.
            task.Run();
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < repetitionCount; i++)
            {
                task.Run();
            }
            stopwatch.Stop();

            double durationInSeconds = stopwatch.Elapsed.TotalMilliseconds / repetitionCount;

            return durationInSeconds;
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var benchmark = new Benchmark();

            var stringTask = new StringTask();

            var builderTask = new BuilderTask();

            var stringResult = benchmark.MeasureDurationInMs(stringTask, 10000);

            var builderResult = benchmark.MeasureDurationInMs(builderTask, 10000);

            Assert.Less(stringResult, builderResult);
        }
    }

    public class StringTask : ITask
    {
        public void Run()
        {
            new string('a', 1);
        }
    }

    public class BuilderTask : ITask
    {
        private StringBuilder sb = new StringBuilder();

        public void Run()
        {
            sb.Append("a").ToString();
        }
    }
}