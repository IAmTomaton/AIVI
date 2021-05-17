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
            var racer = new RandomRacer(7);
            //var racer = new RandomRacer(7, 5, 6);
            var score = 0.0;
            var evaluationFunction = new EndRaceEvaluationFunction();
            var test = RaceProblemsRepo.GetTests(false).ElementAt(0);
            var state = RaceController.Play(test, racer, true);
            Console.WriteLine(score);
            Console.WriteLine(Path.Combine(TestContext.CurrentContext.TestDirectory, "racing", "visualizer", "index.html"));
        }

        [Test]
        [Explicit("TuneRandomRace")]
        public void TuneRandomRace()
        {
            var tuner = new TunerDepthRandomRacer();
            var evaluationFunction = new EndRaceEvaluationFunction();
            var numberCycles = 60;
            ComparisonResult initData = ResultLogger.ReadResult("result_tune_random.txt");
            for (var i = 0; i < numberCycles; i++)
            {
                initData = tuner.Tune(evaluationFunction, testSet, 6, initData);
                ResultLogger.LogResult(initData, "result_tune_random.txt");
            }
        }

        [Test]
        [Explicit("TuneRandomRaceConstantDepth")]
        public void TuneRandomRaceConstantDepth()
        {
            var tuner = new TunerConstantDepthRandom();
            var evaluationFunction = new EndRaceEvaluationFunction();
            var numberCycles = 40;
            ComparisonResult initData = ResultLogger.ReadResult("result_tune_random_constant_depth.txt");
            for (var i = 0; i < numberCycles; i++)
            {
                initData = tuner.Tune(evaluationFunction, testSet, 6, initData);
                ResultLogger.LogResult(initData, "result_tune_random_constant_depth.txt");
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
        [Explicit("RRClone")]
        public void RRClone()
        {
            var evaluationFunction = new EndRaceEvaluationFunction();

            var solvers = new Dictionary<string, Func<(int, bool), RaceState>>
            {
                {
                    "RRClone", i =>
                    {
                        var racer = new RRClone();
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                }
            };

            var numberCycles = 20;
            ComparisonResult initData = ResultLogger.ReadResult("result_rrclone.txt");
            for (var i = 0; i < numberCycles; i++)
            {
                initData = Comparator.Compare(solvers, testSet, evaluationFunction, 6, initData);
                ResultLogger.LogResult(initData, "result_rrclone.txt");
            }
        }

        [Test]
        [Explicit("CompareTune")]
        public void CompareTune()
        {
            var evaluationFunction = new EndRaceEvaluationFunction();

            var solvers = new Dictionary<string, Func<(int, bool), RaceState>>
            {
                {
                    "maxDepth:7 depthDivider:5 minDepth:6", i =>
                    {
                        var racer = new RandomRacer(7, 5, 6);
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                }
            };

            var numberCycles = 20;
            ComparisonResult initData = ResultLogger.ReadResult("result_compare_tune.txt");
            for (var i = 0; i < numberCycles; i++)
            {
                initData = Comparator.Compare(solvers, testSet, evaluationFunction, 4, initData);
                ResultLogger.LogResult(initData, "result_compare_tune.txt");
            }
        }

        [Test]
        [Explicit("Compare")]
        public void Compare()
        {
            var evaluationFunction = new EndRaceEvaluationFunction();

            var solvers = new Dictionary<string, Func<(int, bool), RaceState>>
            {
                //{
                //    "RND7", i =>
                //    {
                //        var racer = new RandomRacer(7);
                //        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                //        var state = RaceController.Play(test, racer, false);
                //        return state;
                //    }
                //},
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
                    "HC RND7", i =>
                    {
                        var racer = new HillClimbingRacer(new RandomRacer(7));
                        var test = RaceProblemsRepo.GetTests(i.Item2).ElementAt(i.Item1);
                        var state = RaceController.Play(test, racer, false);
                        return state;
                    }
                }
            };

            var numberCycles = 10;
            ComparisonResult initData = ResultLogger.ReadResult("result_compare_3.1.txt");
            for (var i = 0; i < numberCycles; i++)
            {
                initData = Comparator.Compare(solvers, testSet, evaluationFunction, 6, initData, true);
                ResultLogger.LogResult(initData, "result_compare_3.1.txt");
            }
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