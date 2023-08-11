using System;
using System.Collections.Generic;

namespace StructBenchmarking
{
    public class Experiments
    {
        private static List<int> list = new List<int>();

        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();

            var structuresTimes = new List<ExperimentResult>();

            foreach (var element in Constants.FieldCounts)
            {
                list.Add(element);
            }

            for (int i = 0; i < Constants.FieldCounts.Count; i++)
            {
                classesTimes.Add(new ExperimentResult(list[i], benchmark.MeasureDurationInMs(new ClassArrayCreationTask(list[i]), repetitionsCount)));
                structuresTimes.Add(new ExperimentResult(list[i], benchmark.MeasureDurationInMs(new StructArrayCreationTask(list[i]), repetitionsCount)));
            }

            list.Clear();

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();


            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}