using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiAlgorithms.racing
{
    class StatisticsСollector
    {
        public static (double average, double dispersion) Test(int trialsCount, Func<double> func)
        {
            var results = Enumerable.Range(0, trialsCount).Select(_ => func());
            var average = GetAverage(results, trialsCount);
            var dispersion = Dispersion(results, average, trialsCount);
            return (Math.Round(average, 2), Math.Round(Math.Sqrt(dispersion), 2));
        }

        private static double GetAverage(IEnumerable<double> values, int count)
        {
            return values.Sum() / count;
        }

        private static double Dispersion(IEnumerable<double> values, double average, int count)
        {
            return values.Select(value => (average - value) * (average - value)).Sum() / count;
        }
    }
}
