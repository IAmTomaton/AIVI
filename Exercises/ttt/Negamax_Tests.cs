using System;
using System.Diagnostics;
using NUnit.Framework;

namespace AiAlgorithms.ttt
{
    [TestFixture]
    public class Negamax_Tests : IScoredTest
    {
        [Test]
        public void Play()
        {
            var wins = CalculateScore();
            Assert.Greater(wins, 1, "You should win at least one game in 7 seconds");
        }

        public double CalculateScore()
        {
            var me = TttPlayer.X;
            var controller = new TttController(3);
            var wins = 0;
            var random = new Random(123412312);
            var sw = Stopwatch.StartNew();
            var gamesCount = 0;
            while (sw.Elapsed < TimeSpan.FromSeconds(7))
            {
                var negamax = new NegamaxSolver(me);
                var greedy = new GreedyTttSolver(me.Opponent(), random);
                var board = me == TttPlayer.X
                    ? controller.Play(negamax, greedy, false)
                    : controller.Play(greedy, negamax, false);
                var winner = board.GetFullLine();
                if (winner == me)
                    wins++;
                else if (winner != TttPlayer.NA)
                    Assert.Fail("You lose in TTT!");
                me = me.Opponent();
                gamesCount++;
            }
            Console.WriteLine($"{wins} wins in {gamesCount} games.");
            return wins;
        }

        public double MinScoreToPassTest { get; }
    }
}