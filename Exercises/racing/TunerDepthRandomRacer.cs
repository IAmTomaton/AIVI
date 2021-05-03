using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AiAlgorithms.racing
{
    class TunerDepthRandomRacer : ITuner<(int, bool), RaceState>
    {
        public ComparisonResult Tune(IEvaluationFunction<RaceState> evaluationFunction, List<(int, bool)> testSet, int trialsCount, ComparisonResult initialData = null)
        {
            var solutions = new Dictionary<string, Func<(int, bool), RaceState>>();

            for (var maxDepth = 5; maxDepth <= 15; maxDepth++)
            {
                for (var depthDivider = 2; depthDivider <= 5; depthDivider++)
                {
                    for (var minDepth = 2; minDepth <= 10; minDepth++)
                    {
                        if (maxDepth < minDepth)
                            continue;

                        var tempMaxDepth = maxDepth;
                        var tempDepthDivider = depthDivider;
                        var tempMinDepth = minDepth;
                        solutions.Add($"maxDepth:{maxDepth} depthDivider:{depthDivider} minDepth:{minDepth}", i =>
                        {
                            var racer = new RandomRacer(tempMaxDepth, tempDepthDivider, tempMinDepth);
                            var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                            var state = RaceController.Play(test, racer, false);
                            return state;
                        });
                    }
                }
            }

            var result = Comparator.Compare(solutions, testSet, evaluationFunction, trialsCount, initialData);

            return result;
        }
    }
}
