using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class NaiveRacer : ISolver<RaceState, RaceSolution>
    {
        public IEnumerable<RaceSolution> GetSolutions(RaceState problem, Countdown countdown)
        {
            var car = problem.Car;
            var distanceToFlag = problem.GetFlagFor(car).DistTo(car.Pos);
            var depth = 5;

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

            var pathCount = 100;
            var paths = new List<V[]>();
            var random = new Random();

            var bestPathIndex = 0;
            var value = double.NegativeInfinity;

            for (var i = 0; i < pathCount; i++)
            {
                var path = new V[depth].Select(_ => directions[random.Next(0, directions.Length)]).ToArray();
                paths.Add(path);
                var newValue = Simulation(problem.MakeCopy(), path);
                if (value < newValue)
                {
                    value = newValue;
                    bestPathIndex = i;
                }
            }

            for (var i = 0; i < pathCount; i++)
            {
                if (bestPathIndex != i)
                {
                    yield return new RaceSolution(paths[i]);
                }
            }

            yield return new RaceSolution(paths[bestPathIndex]);
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