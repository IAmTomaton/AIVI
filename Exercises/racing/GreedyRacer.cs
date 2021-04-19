using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class GreedyRacer : ISolver<RaceState, RaceSolution>
    {
        public IEnumerable<RaceSolution> GetSolutions(RaceState problem, Countdown countdown)
        {
            var car = problem.Car;
            var distanceToFlag = problem.GetFlagFor(car).DistTo(car.Pos);
            var depth = Math.Min(10, (int)distanceToFlag / 3 + 1);

            var directions = new V[9];

            var index = 0;
            for (var dx = -1; dx < 2; dx++)
            {
                for (var dy = -1; dy < 2; dy++)
                {
                    directions[index] = new V(dx, dy);
                    index++;
                }
            }

            V bestCommand = null;
            var value = 0.0;

            foreach (var dir in directions)
            {
                var newValue = Simulation(problem.MakeCopy(), dir, depth);
                if (bestCommand is null || value < newValue)
                {
                    value = newValue;
                    bestCommand = dir;
                }
            }

            foreach (var dir in directions)
            {
                if (bestCommand != dir)
                {
                    yield return new RaceSolution(Enumerable.Range(0, depth).Select(_ => dir).ToArray());
                }
            }

            yield return new RaceSolution(Enumerable.Range(0, depth).Select(_ => bestCommand).ToArray());
        }

        private double Simulation(RaceState problem, V dir, int depth)
        {
            var flagCost = 1000;
            var distanceCost = 1;

            for (var i = 0; i < depth; i++)
            {
                problem.Car.NextCommand = dir;
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