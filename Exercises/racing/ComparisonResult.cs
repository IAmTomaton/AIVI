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
        public SolverStat BestSolverStatistics;
        public Dictionary<string, SolverStat> Adjacent;
        public Dictionary<string, SolverStat> Other;

        public Dictionary<string, SolverStat> InBunch()
        {
            var allStat = Adjacent.Concat(Other).ToDictionary(x => x.Key, x => x.Value);
            allStat.Add(BestSolverName, BestSolverStatistics);
            return allStat;
        }
    }
}
