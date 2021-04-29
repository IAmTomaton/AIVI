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
            var task = Task.WhenAll(Enumerable.Range(0, trialsCount).Select(_ =>
                Task.WhenAll(
                    testSet.Select(
                        test => Task.Run(() => evaluationFunction.Evaluate(func(test)))))
                .ContinueWith(task => task.Result.Sum())
            ));

            task.Wait();
            return new StatValue(task.Result);
        }
    }
}
