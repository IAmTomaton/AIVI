using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.ttt
{
    public enum TttPlayer
    {
        X,
        O,
        NA,
    }

    public static class TttPlayerExtensions
    {
        public static TttPlayer Opponent(this TttPlayer player)
        {
            return player switch
            {
                TttPlayer.O => TttPlayer.X,
                TttPlayer.X => TttPlayer.O,
                _ => throw new InvalidOperationException(player.ToString())
            };
        }
    }

    public class BoardMove : ISolution
    {
        public double Score { get; }
        public int CellIndex;

        public BoardMove(in int cellIndex, in double score)
        {
            CellIndex = cellIndex;
            Score = score;
        }

        public override string ToString()
        {
            return $"({CellIndex % 3}, {CellIndex / 3}) with score {Score}";
        }
    }

    public struct Board
    {
        public int Size { get; }
        private readonly int xs;
        private readonly int os;

        public Board(int size) 
            : this(size, 0, 0)
        {
            this.Size = size;
        }

        private Board(int size, int xs, int os)
        {
            this.Size = size;
            this.xs = xs;
            this.os = os;
        }

        public List<int> GetPossibleMoves()
        {
            var res = new List<int>(32);
            var boardSize = Size * Size;
            for (int pos = 0, occupied = xs | os;
                pos < boardSize;
                pos++, occupied >>= 1)
            {
                if ((occupied & 1) == 0) res.Add(pos);
            }
            return res;
        }

        public TttPlayer GetCell(int x, int y)
        {
            var cellIndex = x + y * Size;
            return GetCell(cellIndex);
        }

        public TttPlayer GetCell(int cellIndex)
        {
            if (xs.GetBit(cellIndex) == 1) return TttPlayer.X;
            if (os.GetBit(cellIndex) == 1) return TttPlayer.O;
            return TttPlayer.NA;
        }

        public Board Move(TttPlayer player, int cellIndex)
        {
            return player == TttPlayer.X
                ? new Board(Size, xs.SetBit(cellIndex), os)
                : new Board(Size, xs, os.SetBit(cellIndex));
        }

        public Board Move(TttPlayer player, int x, int y)
        {
            return Move(player, x + y * Size);
        }

        public IEnumerable<(int xCount, int oCount)> AnalyzeLines()
        {
            foreach (var line in GetAllLines())
            {
                var xCount = 0;
                var yCount = 0;
                for (var i = 0; i < 3; i++)
                {
                    var cell = GetCell(line[i]);
                    if (cell == TttPlayer.X) xCount++;
                    else if (cell == TttPlayer.O) yCount++;
                }

                yield return (xCount, yCount);
            }
        }

        private IEnumerable<IList<int>> GetAllLines()
        {
            var size = Size;
            for (int x = 0; x < size; x++)
                yield return Enumerable.Range(0, Size).Select(y => x + y * size).ToList();
            for (int y = 0; y < size; y++)
                yield return Enumerable.Range(0, Size).Select(x => x + y * size).ToList();
            yield return Enumerable.Range(0, Size).Select(i => i + i*size).ToList();
            yield return Enumerable.Range(0, Size).Select(i => i*size + size - 1 - i).ToList();
        }

        public TttPlayer GetFullLine()
        {
            var size = Size;
            var fullLines = AnalyzeLines()
                .Select(line => line.xCount == size ? TttPlayer.X : line.oCount == size ? TttPlayer.O : TttPlayer.NA)
                .Where(winner => winner != TttPlayer.NA)
                .DefaultIfEmpty(TttPlayer.NA);
            return fullLines.First();
        }

        public bool IsFull()
        {
            var fullBoard = int.MaxValue >> (31 - Size * Size);
            return (os | xs) == fullBoard;
        }

        public bool IsEmpty()
        {
            return (os | xs) == 0;
        }
    }
}
