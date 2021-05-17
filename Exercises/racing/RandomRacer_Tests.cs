using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace AiAlgorithms.racing
{
    [TestFixture]
    public class RandomRacer_Tests
    {
        public static List<(int, bool)> testSet = new List<(int, bool)> { (0, true), (1, true), (2, true), (0, false), (1, false), (2, false) };

        [Test]
        [Explicit("Тест для отладки и анализа")]
        public void VisualizeRace()
        {
            //var racer = new GreedyRacer(9, 5, 5);
            var racer = new RandomRacer(10, 4, 5);
            //var racer = new RandomRacer(7, 5, 6);
            var test = RaceProblemsRepo.GetTests(true).ElementAt(0);
            RaceController.Play(test, racer, true);
            Console.WriteLine(Path.Combine(TestContext.CurrentContext.TestDirectory, "racing", "visualizer", "index.html"));
        }

        [Test]
        [Explicit("TuneRandomRace")]
        public void TuneRandomRace()
        {
            var tuner = new TunerDepthRandomRacer();
            var evaluationFunction = new EndRaceEvaluationFunction();
            var numberCycles = 1;
            ComparisonResult initData = ResultLogger.ReadResult("result_tune_random.txt");
            for (var i = 0; i < numberCycles; i++)
            {
                initData = tuner.Tune(evaluationFunction, testSet, 1, initData);
                ResultLogger.LogResult(initData, "result_tune_random.txt");
            }
        }

        [Test]
        [Explicit("TuneGreedyRace")]
        public void TuneGreedyRace()
        {
            var tuner = new TunerDepthGreedyRacer();
            var evaluationFunction = new EndRaceEvaluationFunction();
            var result = tuner.Tune(evaluationFunction, testSet, 1);
            ResultLogger.LogResult(result, "result_tune_greedy.txt");
        }

        [Test]
        [Explicit("Compare")]
        public void Compare()
        {
            var trialsCount = 10;
            var evaluationFunction = new EndRaceEvaluationFunction();

            var result = Comparator.Compare(new Dictionary<string, Func<(int, bool), RaceState>>
            {
                {
                    "RND", i =>
                    {
                        var racer = new RandomRacer();
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                },
                {
                    "Greedy", i =>
                    {
                        var racer = new GreedyRacer(9, 5, 5);
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                },
                {
                    "Native", i =>
                    {
                        var racer = new NaiveRacer();
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                }
            },
            testSet, evaluationFunction, trialsCount);

            ResultLogger.LogResult(result, "result_compare.txt");
        }

        [Test]
        [Explicit("CompareGready")]
        public void CompareGready()
        {
            var trialsCount = 1;
            var evaluationFunction = new EndRaceEvaluationFunction();
            var initData = ResultLogger.ReadResult("result_compare_gready.txt");

            var result = Comparator.Compare(new Dictionary<string, Func<(int, bool), RaceState>>
            {
                {
                    "Greedy1", i =>
                    {
                        var racer = new GreedyRacer(9, 5, 5);
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                },
                {
                    "Greedy2", i =>
                    {
                        var racer = new GreedyRacer(10, 5, 5);
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                }
            },
            testSet, evaluationFunction, trialsCount, initData);

            ResultLogger.LogResult(result, "result_compare_gready.txt");
        }
    }
}