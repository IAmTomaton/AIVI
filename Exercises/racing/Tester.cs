using AiAlgorithms.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAlgorithms.racing
{
    class Tester
    {
        public static StatValue Test<TSet, TState>(Func<TSet, TState> func, List<TSet> testSet,
            IEvaluationFunction<TState> evaluationFunction, int trialsCount)
        {
            var tasks = Enumerable.Range(0, trialsCount)
                .Select(_ => Task.Run(() => testSet.Select(test => evaluationFunction.Evaluate(func(test))).Sum()))
                .ToArray();
            Task.WaitAll(tasks);
            return new StatValue(tasks.Select(task => task.Result));
        }
    }
}
