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
            output.AppendLine("Name\tAverage\tDeviation\tLowerBound\tUpperBound");
            output.AppendLine("Best");
            output.AppendLine($"{result.BestSolutionName}\t{result.BestSolutionStatistics.GetString()}");

            output.AppendLine("Adjacent");
            foreach (var solution in result.Adjacent)
            {
                output.AppendLine($"{solution.Key}\t{solution.Value.GetString()}");
            }

            output.AppendLine("Other");
            foreach (var solution in result.Other)
            {
                output.AppendLine($"{solution.Key}\t{solution.Value.GetString()}");
            }

            using (StreamWriter sw = new StreamWriter($"{fileName}", false, Encoding.Default))
            {
                sw.Write(output.ToString());
            }
        }
    }
}
