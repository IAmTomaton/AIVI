using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.ttt
{
    public class TttController
    {
        private readonly int size;

        public TttController(int size)
        {
            this.size = size;
        }

        public Board Play(ISolver<Board, BoardMove> xPlayer, ISolver<Board, BoardMove> oPlayer, bool makeLog)
        {
            var board = new Board(size);
            var players = new[] {xPlayer, oPlayer};
            var currentPlayerIndex = 0;
            while (!board.IsFull() && board.GetFullLine() == TttPlayer.NA)
            {
                var moves = players[currentPlayerIndex].GetSolutions(board, 100).ToList();
                board = board.Move((TttPlayer) currentPlayerIndex, moves.Last().CellIndex);
                currentPlayerIndex = 1 - currentPlayerIndex;
                if (makeLog) PrintBoard(board, moves);
            }
            return board;
        }

        private void PrintBoard(Board board, List<BoardMove> moves)
        {
            var boardMap = Enumerable.Range(0, board.Size)
                .Select(y => Enumerable.Range(0, board.Size)
                    .Select(x => FormatCell(board.GetCell(x, y))).StrJoin(""))
                .StrJoin("\n");

            Console.WriteLine(boardMap);
            foreach (var move in moves)
                Console.WriteLine(move);
        }

        private string FormatCell(TttPlayer cell)
        {
            if (cell == TttPlayer.NA) return ".";
            if (cell == TttPlayer.O) return "O";
            if (cell == TttPlayer.X) return "X";
            throw new Exception(cell.ToString());
        }
    }
}