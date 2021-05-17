using AiAlgorithms.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiAlgorithms.racing
{
    class SolverStat
    {
        public StatValue ScoreStat;
        public StatValue NegInfStat;

        public SolverStat(StatValue scoreStat, StatValue negInfStat)
        {
            ScoreStat = scoreStat;
            NegInfStat = negInfStat;
        }

        public void Add(SolverStat other)
        {
            ScoreStat.AddAll(other.ScoreStat);
            NegInfStat.AddAll(other.NegInfStat);
        }

        public string Serialization()
        {
            return $"{ScoreStat.Serialization()}\t{NegInfStat.Serialization()}";
        }

        public static SolverStat Deserialization(string[] numbers)
        {
            return new SolverStat(
                StatValue.Deserialization(numbers.Take(numbers.Length / 2).ToArray()),
                StatValue.Deserialization(numbers.Skip(numbers.Length / 2).Take(numbers.Length / 2).ToArray()));
        }
    }
}
