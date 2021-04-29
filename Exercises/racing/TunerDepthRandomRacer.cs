using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AiAlgorithms.racing
{
    class TunerDepthRandomRacer : ITuner<RaceState>
    {
        public ComparisonResult Tune(IEvaluationFunction<RaceState> evaluationFunction, int trialsCount, ComparisonResult initialData = null)
        {
            var solutions = new Dictionary<string, Func<(int, bool), RaceState>>();

            for (var maxDepth = 9; maxDepth <= 11; maxDepth++)
            {
                for (var depthDivider = 4; depthDivider <= 4; depthDivider++)
                {
                    for (var minDepth = 5; minDepth <= 5; minDepth++)
                    {
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

            var result = Comparator.Compare(solutions, new List<(int, bool)> { (0, true) }, evaluationFunction, trialsCount, initialData);

            return result;
        }
    }
}
