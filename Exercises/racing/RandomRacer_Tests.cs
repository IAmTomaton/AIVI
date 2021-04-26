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
            // Открой файл bin/Debug/*/racing/visualizer/index.html чтобы посмотреть реплей на тесте testIndex
            var racer = new GreedyRacer(9, 5, 5);
            //var racer = new RandomRacer();
            var test = RaceProblemsRepo.GetTests(true).ElementAt(2);
            RaceController.Play(test, racer, true);
            Console.WriteLine(Path.Combine(TestContext.CurrentContext.TestDirectory, "racing", "visualizer", "index.html"));
        }

        [Test]
        [Explicit("TuneRace")]
        public void TuneRandomRace()
        {
            var tuner = new TunerDepthRandomRacer();
            var evaluationFunction = new EndRaceEvaluationFunction();
            var result = tuner.Tune(evaluationFunction, 5);
            ResultLogger.LogResult(result, "result_tune_random.txt");
        }

        [Test]
        [Explicit("TuneRace")]
        public void TuneGreedyRace()
        {
            var tuner = new TunerDepthGreedyRacer();
            var evaluationFunction = new EndRaceEvaluationFunction();
            var result = tuner.Tune(evaluationFunction, 10);
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
            }, new List<(int, bool)> { (0, true), (1, true), (2, true) }, evaluationFunction, trialsCount);

            ResultLogger.LogResult(result, "result_compare.txt");
        }

        [Test]
        [Explicit("Compare")]
        public void CompareGready()
        {
            var trialsCount = 10;
            var evaluationFunction = new EndRaceEvaluationFunction();

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
                    "Greedy1", i =>
                    {
                        var racer = new GreedyRacer(15, 3, 7);
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                }
            }, new List<(int, bool)> { (0, true), (1, true), (2, true) }, evaluationFunction, trialsCount);

            ResultLogger.LogResult(result, "result_compare.txt");
        }
    }
}