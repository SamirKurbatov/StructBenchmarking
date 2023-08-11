using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StructBenchmarking
{
    interface IExperiment
    {
        List<ExperimentResult> RunExperiment(IBenchmark benchmark, int repeationCount);
    }

    abstract class AbstractExperiment : IExperiment
    {
        public List<ExperimentResult> RunExperiment(IBenchmark benchmark, int repeationCount)
        {
            var list = new List<ExperimentResult>();

            var fields = Constants.FieldCounts;

            foreach (var item in fields)
            {
                var averageTime = benchmark.MeasureDurationInMs(GetTask(item), repeationCount);
                list.Add(new ExperimentResult(item, averageTime));
            }

            return list;
        }

        public abstract ITask GetTask(int size);
    }

    class ArrayExperiment : AbstractExperiment
    {
        public override ITask GetTask(int size)
        {
            return new ClassArrayCreationTask(size);
        }
    }

    class StructExperiment : AbstractExperiment
    {
        public override ITask GetTask(int size)
        {
            return new StructArrayCreationTask(size);
        }
    }

    class MethodCallWithClassArgumentTaskExperiment : AbstractExperiment
    {
        public override ITask GetTask(int size)
        {
            return new MethodCallWithClassArgumentTask(size);
        }
    }

    class MethodCallWithStructArgumentTaskExperiment : AbstractExperiment
    {
        public override ITask GetTask(int size)
        {
            return new MethodCallWithStructArgumentTask(size);
        }
    }

    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repeationCount)
        {
            var array = new ArrayExperiment();

            var structExperiment = new StructExperiment();

            var classResultListExperiment = array.RunExperiment(benchmark, repeationCount);

            var structResultListExperiment = structExperiment.RunExperiment(benchmark, repeationCount);

            return new ChartData()
            {
                ClassPoints = classResultListExperiment,
                StructPoints = structResultListExperiment,
                Title = "Create array"
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repeationCount)
        {
            var methodClass = new MethodCallWithClassArgumentTaskExperiment();

            var methodStruct = new MethodCallWithStructArgumentTaskExperiment();

            var classResultListExperiment = methodClass.RunExperiment(benchmark, repeationCount);

            var structResultListExperiment = methodStruct.RunExperiment(benchmark, repeationCount);

            return new ChartData()
            {
                ClassPoints = classResultListExperiment,
                StructPoints = structResultListExperiment,
                Title = "Call method with argument"
            };
        }
    }
}