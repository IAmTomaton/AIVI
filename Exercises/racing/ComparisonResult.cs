using System;
using System.Collections.Generic;
using System.Text;

namespace AiAlgorithms.racing
{
    struct ComparisonResult
    {
        public string BestSolutionName;
        public SolutionStatistics BestSolutionStatistics;
        public Dictionary<string, SolutionStatistics> Adjacent;
        public Dictionary<string, SolutionStatistics> Other;
    }
}
