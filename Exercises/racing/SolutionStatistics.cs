using System;
using System.Collections.Generic;
using System.Linq;

namespace AiAlgorithms.racing
{
    class SolutionStatistics
    {
        public double Average { get; }
        public double Deviation { get; }
        public double LowerBound { get; }
        public double UpperBound { get; }

        public SolutionStatistics(List<double> values)
        {
            Average = GetAverage(values);
            Deviation = GetStandardDeviation(values, Average);
            LowerBound = Math.Round(Average - Deviation, 2);
            UpperBound = Math.Round(Average + Deviation, 2);
        }

        public static double GetAverage(List<double> values)
        {
            return Math.Round(values.Sum() / values.Count, 2);
        }

        public static double GetStandardDeviation(List<double> values, double average)
        {
            return Math.Round(Math.Sqrt(values.Select(value => (average - value) * (average - value)).Sum() / values.Count), 2);
        }

        public string GetString() => $"{Average}\t{Deviation}\t{LowerBound}\t{UpperBound}";
    }
}
