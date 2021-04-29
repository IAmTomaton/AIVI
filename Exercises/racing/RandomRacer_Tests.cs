using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AiAlgorithms.racing
{
    [TestFixture]
    public class RandomRacer_Tests
    {
        [Test]
        [Explicit("Тест для отладки и анализа")]
        public void VisualizeRace()
        {
            //var racer = new GreedyRacer(9, 5, 5);
            var numberCycles = 50;
            for (var i = 0; i < numberCycles; i++)
            {
                var racer = new RandomRacer(10, 4, 5);
                var test = RaceProblemsRepo.GetTests(true).ElementAt(0);
                if (!RaceController.Play(test, racer, true).Car.IsAlive)
                    break;
            }
            Console.WriteLine(Path.Combine(TestContext.CurrentContext.TestDirectory, "racing", "visualizer", "index.html"));
        }

        [Test]
        [Explicit("TuneRace")]
        public void TuneRandomRace()
        {
            var tuner = new TunerDepthRandomRacer();
            var evaluationFunction = new EndRaceEvaluationFunction();
            var numberCycles = 10;
            ComparisonResult initData = null;
            for (var i = 0; i < numberCycles; i++)
            {
                initData = tuner.Tune(evaluationFunction, 5, initData);
                ResultLogger.LogResult(initData, "result_tune_random.txt");
            }
        }

        [Test]
        [Explicit("TuneRace")]
        public void TuneGreedyRace()
        {
            var tuner = new TunerDepthGreedyRacer();
            var evaluationFunction = new EndRaceEvaluationFunction();
            var result = tuner.Tune(evaluationFunction, 1);
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
            }, new List<(int, bool)> { (0, true), (1, true), (2, true), (0, false), (1, false), (2, false) }, evaluationFunction, trialsCount);

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
            new List<(int, bool)> { (0, true), (1, true), (2, true), (0, false), (1, false), (2, false) },
            evaluationFunction, trialsCount, initData);

            ResultLogger.LogResult(result, "result_compare_gready.txt");
        }
    }
}