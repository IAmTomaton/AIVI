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
        public void VisualizeRace([Values(0)]int testIndex)
        {
            // Открой файл bin/Debug/*/racing/visualizer/index.html чтобы посмотреть реплей на тесте testIndex
            var racer = new RandomRacer();
            var test = RaceProblemsRepo.GetTests(true).ElementAt(testIndex);
            RaceController.Play(test, racer, true);
            Console.WriteLine(Path.Combine(TestContext.CurrentContext.TestDirectory, "racing", "visualizer", "index.html"));
        }

        [Test]
        [Explicit("Тест для отладки и анализа")]
        public void TuneRace([Values(0)] int testIndex)
        {
            // Открой файл bin/Debug/*/racing/visualizer/index.html чтобы посмотреть реплей на тесте testIndex
            var tuner = new TunerRandomRacer();
            var result = tuner.Tune(10);
            ResultLogger.LogResult(result, "result.txt");
        }
    }
}