using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class HillClimbingRandomRacer : ISolver<RaceState, RaceSolution>
    {
        private V[] directions = new V[9];
        private Random random = new Random();

        public HillClimbingRandomRacer()
        {
            var index = 0;
            for (var dx = -1; dx < 2; dx++)
            {
                for (var dy = -1; dy < 2; dy++)
                {
                    directions[index] = new V(dx, dy);
                    index++;
                }
            }
        }

        public IEnumerable<RaceSolution> GetSolutions(RaceState problem, Countdown countdown)
        {
            var car = problem.Car;
            var distanceToFlag = problem.GetFlagFor(car).DistTo(car.Pos);
            var depth = Math.Min(10, (int)distanceToFlag / 4 + 5);

            V[] bestPath = null;
            var value = double.NegativeInfinity;

            while (!countdown.IsFinished())
            {
                var path = Enumerable.Range(0, depth).Select(_ => directions[random.Next(0, directions.Length)]).ToArray();
                var newValue = Simulation(problem.MakeCopy(), path);
                if (value < newValue)
                {
                    yield return new RaceSolution(path);
                    value = newValue;
                    bestPath = path;
                }
            }
        }

        private double Simulation(RaceState problem, V[] commands)
        {
            var flagCost = 1000;
            var distanceCost = 1;

            foreach (var command in commands)
            {
                problem.Car.NextCommand = command;
                problem.Tick();
                if (!problem.Car.IsAlive)
                    return double.NegativeInfinity;
                if (problem.IsFinished)
                    break;
            }
            var car = problem.Car;
            var value = car.FlagsTaken * flagCost - car.Pos.DistTo(problem.GetFlagFor(car)) * distanceCost;

            return value;
        }
    }
}