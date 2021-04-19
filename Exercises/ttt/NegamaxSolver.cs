using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.ttt
{
    public class NegamaxSolver : ISolver<Board, BoardMove>
    {
        public NegamaxSolver(TttPlayer me)
        {
        }

        public IEnumerable<BoardMove> GetSolutions(Board board, Countdown countdown)
        {
            throw new NotImplementedException();
        }
    }
}