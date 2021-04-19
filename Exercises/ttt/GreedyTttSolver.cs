using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.ttt
{
    public class GreedyTttSolver : ISolver<Board, BoardMove>
    {
        private readonly TttPlayer me;
        private readonly Random random;

        public GreedyTttSolver(TttPlayer me, Random random)
        {
            this.me = me;
            this.random = random;
        }

        public IEnumerable<BoardMove> GetSolutions(Board board, Countdown countdown)
        {
            var scoredMoves =
                (from move in board.GetPossibleMoves()
                 let finalBoard = board.Move(me, move)
                 let score = Estimate(finalBoard)
                 orderby score
                 select new BoardMove(move, score)).ToList();
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var bestMoves = scoredMoves.Where(m => m.Score == scoredMoves.Last().Score).Shuffle(random).ToList();
            return bestMoves;
        }

        private double Estimate(Board board)
        {
            var score = 0;
            foreach (var line in board.AnalyzeLines())
            {
                var myIndex = (int)me;
                var hisIndex = 1 - myIndex;
                var counts = new[] { line.xCount, line.oCount };
                if (counts[myIndex] == 3) score += 1000;
                if (counts[hisIndex] == 3) score -= 10000;
                if (counts[myIndex] == 2 && counts[hisIndex] == 0) score += 1;
                if (counts[hisIndex] == 2 && counts[myIndex] == 0) score -= 100;
            }
            return score;
        }
    }
}