using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAlgorithms.racing
{
    class Tester
    {
        public static SolutionStatistics Test<TSet, TState>(Func<TSet, TState> func, List<TSet> testSet,
            IEvaluationFunction<TState> evaluationFunction, int trialsCount)
        {
            var tasks = testSet.SelectMany(test => Enumerable.Range(0, trialsCount).Select(_ => Task.Run(() => func(test))));

            var values = tasks.Select(task =>
            {
                task.Wait();
                return evaluationFunction.Evaluate(task.Result);
            }).ToList();

            return new SolutionStatistics(values);
        }
    }
}
