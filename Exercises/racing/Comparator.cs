using AiAlgorithms.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAlgorithms.racing
{
    class Comparator
    {
        public static ComparisonResult Compare<TSet, TState>(Dictionary<string, Func<TSet, TState>> funcs, TSet test,
            IEvaluationFunction<TState> evaluationFunction, int trialsCount, ComparisonResult initialData=null, bool filterNegativeInfinity = false)
        {
            return Compare(funcs, new List<TSet> { test }, evaluationFunction, trialsCount, initialData, filterNegativeInfinity);
        }

        public static ComparisonResult Compare<TSet, TState>(Dictionary<string, Func<TSet, TState>> funcs, List<TSet> testSet,
            IEvaluationFunction<TState> evaluationFunction, int trialsCount, ComparisonResult initialData=null, bool filterNegativeInfinity = false)
        {
            var results = MakeTests(funcs, testSet, evaluationFunction, trialsCount, filterNegativeInfinity);
            if (!(initialData is null))
            {
                var initialStats = initialData.InBunch();
                foreach (var key in initialStats.Keys)
                {
                    if (results.ContainsKey(key))
                    {
                        results[key].Add(initialStats[key]);
                    }
                    else
                        results.Add(key, initialStats[key]);
                }
            }

            (string bestSolution, List<string> adjacentSolutions, List<string> other) = CompareResults(results);

            return new ComparisonResult
            {
                BestSolverName = bestSolution,
                BestSolverStatistics = results[bestSolution],
                Adjacent = adjacentSolutions.ToDictionary(name => name, name => results[name]),
                Other = other.ToDictionary(name => name, name => results[name])
            };
        }

        private static (string bestSolution, List<string> adjacentSolutions, List<string> other) CompareResults(Dictionary<string, SolverStat> results)
        {
            string bestSolution = null;
            var adjacentSolutions = new List<string>();
            var other = new List<string>();

            foreach (var name in results.Keys)
            {
                if (bestSolution is null || results[bestSolution].ScoreStat.Mean < results[name].ScoreStat.Mean)
                    bestSolution = name;
            }

            foreach (var name in results.Keys)
            {
                if (name == bestSolution)
                    continue;
                if (Math.Max(results[bestSolution].ScoreStat.LowerBound, results[name].ScoreStat.LowerBound) <=
                    Math.Min(results[bestSolution].ScoreStat.UpperBound, results[name].ScoreStat.UpperBound) &&
                    results[name].ScoreStat.Mean > double.NegativeInfinity)
                    adjacentSolutions.Add(name);
                else
                    other.Add(name);
            }

            return (bestSolution, adjacentSolutions, other);
        }

        private static Dictionary<string, SolverStat> MakeTests<TSet, TState>(Dictionary<string, Func<TSet, TState>> funcs, List<TSet> testSet,
            IEvaluationFunction<TState> evaluationFunction, int trialsCount, bool filterNegativeInfinity)
        {
            var tasks = new Dictionary<string, Task<SolverStat>>();
            foreach (var name in funcs.Keys)
            {
                var task = Task.Run(() => Tester.Test(funcs[name], testSet, evaluationFunction, trialsCount, filterNegativeInfinity));
                tasks[name] = task;
            }

            var results = new Dictionary<string, SolverStat>();

            foreach (var name in tasks.Keys)
            {
                tasks[name].Wait();
                results.Add(name, tasks[name].Result);
            }

            return results;
        }
    }
}
