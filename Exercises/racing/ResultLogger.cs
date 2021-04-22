using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AiAlgorithms.racing
{
    class ResultLogger
    {
        public static void LogResult(Dictionary<string, List<string>> results, string fileName)
        {
            var output = new StringBuilder();
            output.AppendLine(string.Join('\t', results.Keys));

            var lines = results.Keys.Select(key => results[key].Count).Min();

            for (var i = 0; i < lines; i++)
            {
                output.AppendLine(string.Join('\t', results.Keys.Select(key => results[key][i])));
            }

            using (StreamWriter sw = new StreamWriter($"{fileName}", false, Encoding.Default))
            {
                sw.Write(output.ToString());
            }
        }
    }
}
