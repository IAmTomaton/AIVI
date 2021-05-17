using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiAlgorithms.racing
{
    class TunerConstantDepthRandom : ITuner<(int, bool), RaceState>
    {
        public ComparisonResult Tune(IEvaluationFunction<RaceState> evaluationFunction, List<(int, bool)> testSet, int trialsCount, ComparisonResult initialData = null)
        {
            var solvers = new Dictionary<string, Func<(int, bool), RaceState>>();

            for (var depth = 7; depth <= 7; depth++)
            {
                
                var name = $"depth:{depth}";

                var tempDepth = depth;
                solvers.Add(name, i =>
                {
                    var racer = new RandomRacer(tempDepth);
                    var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                    var state = RaceController.Play(test, racer, false);
                    return state;
                });
            }

            var result = Comparator.Compare(solvers, testSet, evaluationFunction, trialsCount, initialData, true);

            return result;
        }
    }
}
