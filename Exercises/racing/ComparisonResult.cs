using AiAlgorithms.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiAlgorithms.racing
{
    class ComparisonResult
    {
        public string BestSolverName;
        public StatValue BestSolverStatistics;
        public Dictionary<string, StatValue> Adjacent;
        public Dictionary<string, StatValue> Other;

        public Dictionary<string, StatValue> InBunch()
        {
            var allStat = Adjacent.Concat(Other).ToDictionary(x => x.Key, x => x.Value);
            allStat.Add(BestSolverName, BestSolverStatistics);
            return allStat;
        }
    }
}
