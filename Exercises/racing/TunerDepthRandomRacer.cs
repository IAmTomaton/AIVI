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

            for (var maxDepth = 9; maxDepth <= 9; maxDepth++)
            {
                for (var minDepth = 7; minDepth <= 7; minDepth++)
                {
                    for (var depthDivider = 5; depthDivider <= 5; depthDivider++)
                    {
                        var name = $"maxDepth:{maxDepth} depthDivider:{depthDivider} minDepth:{minDepth}";

                        if (maxDepth < minDepth)
                            continue;

                        var tempMaxDepth = maxDepth;
                        var tempDepthDivider = depthDivider;
                        var tempMinDepth = minDepth;
                        solutions.Add(name, i =>
                        {
                            var racer = new RandomRacer(tempMaxDepth, tempDepthDivider, tempMinDepth);
                            var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                            var state = RaceController.Play(test, racer, false);
                            return state;
                        });
                        if (maxDepth == minDepth)
                            break;
                    }
                }
            }

            var result = Comparator.Compare(solutions, testSet, evaluationFunction, trialsCount, initialData, true);

            return result;
        }
    }
}
