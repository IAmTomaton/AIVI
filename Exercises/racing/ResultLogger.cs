using AiAlgorithms.Algorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AiAlgorithms.racing
{
    class ResultLogger
    {
        public static void LogResult(ComparisonResult result, string fileName)
        {
            var output = new StringBuilder();
            output.AppendLine("Best");
            output.AppendLine($"{result.BestSolverName}\t{result.BestSolverStatistics.Serialization()}");

            output.AppendLine($"Adjacent {result.Adjacent.Count}");
            foreach (var solution in result.Adjacent.OrderBy(stat => -stat.Value.ScoreStat.Mean))
            {
                output.AppendLine($"{solution.Key}\t{solution.Value.Serialization()}");
            }

            output.AppendLine($"Other {result.Other.Count}");
            foreach (var solution in result.Other.OrderBy(stat => -stat.Value.ScoreStat.Mean))
            {
                output.AppendLine($"{solution.Key}\t{solution.Value.Serialization()}");
            }

            using (var sw = new StreamWriter($"{fileName}", false, Encoding.Default))
            {
                sw.Write(output.ToString());
            }
        }

        public static ComparisonResult ReadResult(string fileName)
        {
            if (!File.Exists(fileName)) return null;
            using (var sr = new StreamReader($"{fileName}"))
            {
                sr.ReadLine();
                (string nameBest, SolverStat statValueBest) = StatValueFromLine(sr.ReadLine());

                var adjacentCount = int.Parse(sr.ReadLine().Split(' ')[1]);
                var adjacent = Enumerable.Range(0, adjacentCount)
                    .Select(_ => StatValueFromLine(sr.ReadLine()))
                    .ToDictionary(tuple => tuple.name, tuple => tuple.statValue);

                var otherCount = int.Parse(sr.ReadLine().Split(' ')[1]);
                var other = Enumerable.Range(0, otherCount)
                    .Select(_ => StatValueFromLine(sr.ReadLine()))
                    .ToDictionary(tuple => tuple.name, tuple => tuple.statValue);

                return new ComparisonResult
                {
                    BestSolverName = nameBest,
                    BestSolverStatistics = statValueBest,
                    Adjacent = adjacent,
                    Other = other
                };
            }
        }

        private static (string name, SolverStat statValue) StatValueFromLine(string line)
        {
            var arr = line.Split('\t');
            var name = arr[0];
            return (name, SolverStat.Deserialization(arr.Skip(1).ToArray()));
        }
    }
}
