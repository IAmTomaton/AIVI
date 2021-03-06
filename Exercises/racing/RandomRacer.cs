using System;
using System.Collections.Generic;
using System.Linq;
using AiAlgorithms.Algorithms;

namespace AiAlgorithms.racing
{
    public class RandomRacer : ISolver<RaceState, RaceSolution>
    {
        private V[] directions = new V[9];
        private Random random = new Random();
        private int maxDepth = 10;
        private int depthDivider = 4;
        private int minDepth = 5;
        private int constantDepth = -1;

        public RandomRacer()
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

        public RandomRacer(int maxDepth, int depthDivider, int minDepth) : this()
        {
            this.maxDepth = maxDepth;
            this.depthDivider = depthDivider;
            this.minDepth = minDepth;
        }

        public RandomRacer(int depth) : this()
        {
            constantDepth = depth;
        }

        public IEnumerable<RaceSolution> GetSolutions(RaceState problem, Countdown countdown)
        {
            var car = problem.Car;
            var distanceToFlag = problem.GetFlagFor(car).DistTo(car.Pos);
            var depth = constantDepth;
            if (depth == -1)
                depth = Math.Min(maxDepth, Math.Max((int)distanceToFlag / depthDivider, minDepth));

            V[] bestPath = null;
            var value = double.NegativeInfinity;

            while (!countdown.IsFinished())
            {
                var path = Enumerable.Range(0, depth).Select(_ => directions[random.Next(0, directions.Length)]).ToArray();
                var newValue = Simulation(problem.MakeCopy(), path);
                if (value < newValue)
                {
                    var solution = new RaceSolution(path);
                    value = newValue;
                    solution.Score = newValue;
                    bestPath = path;
                    yield return solution;
                }
            }

            if (bestPath is null)
                yield return new RaceSolution(new V[] { new V(-Math.Sign(problem.Car.V.X), -Math.Sign(problem.Car.V.Y)) });
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