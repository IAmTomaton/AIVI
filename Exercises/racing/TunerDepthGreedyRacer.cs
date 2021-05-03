using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AiAlgorithms.racing
{
    class TunerDepthGreedyRacer : ITuner<(int, bool), RaceState>
    {
        public ComparisonResult Tune(IEvaluationFunction<RaceState> evaluationFunction, List<(int, bool)> testSet, int trialsCount, ComparisonResult initialData = null)
        {
            var solutions = new Dictionary<string, Func<(int, bool), RaceState>>();

            for (var maxDepth = 8; maxDepth <= 12; maxDepth++)
            {
                for (var depthDivider = 4; depthDivider <= 5; depthDivider++)
                {
                    for (var minDepth = 3; minDepth <= 7; minDepth++)
                    {
                        var tempMaxDepth = maxDepth;
                        var tempDepthDivider = depthDivider;
                        var tempMinDepth = minDepth;
                        solutions.Add($"maxDepth:{maxDepth} depthDivider:{depthDivider} minDepth:{minDepth}", i =>
                        {
                            var racer = new GreedyRacer(tempMaxDepth, tempDepthDivider, tempMinDepth);
                            var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                            var state = RaceController.Play(test, racer, false);
                            return state;
                        });
                    }
                }
            }

            var result = Comparator.Compare(solutions, testSet, evaluationFunction, trialsCount);

            return result;
        }
    }
}
