using System;
using System.Collections.Generic;
using AiAlgorithms.Algorithms;
using System.Linq;

namespace AiAlgorithms.racing
{
    public class HillClimbingRacer : ISolver<RaceState, RaceSolution>
    {
        private readonly ISolver<RaceState, RaceSolution> baseSolver;
        protected readonly HillClimbingMutator mutator = new HillClimbingMutator();
        private IMutation<RaceSolution> firstMutation;
        private int mutationsCount;
        private int improvementsCount;

        public HillClimbingRacer(ISolver<RaceState, RaceSolution> baseSolver)
        {
            this.baseSolver = baseSolver;
        }

        protected bool ShouldContinue { get; set; }

        public IEnumerable<RaceSolution> GetSolutions(RaceState problem, Countdown countdown)
        {
            mutationsCount = 0;
            improvementsCount = 0;
            ShouldContinue = true;
            var steps = new List<RaceSolution>();
            steps.Add(baseSolver.GetSolutions(problem, countdown / 2).Last());
            while (!countdown.IsFinished())
            {
                var improvements = Improve(problem, steps.Last());
                mutationsCount++;
                foreach (var solution in improvements)
                {
                    improvementsCount++;
                    if (solution is IHaveTime withTime) withTime.Time = countdown.TimeElapsed;
                    if (solution is IHaveIndex withIndex)
                    {
                        withIndex.MutationIndex = mutationsCount;
                        withIndex.ImprovementIndex = improvementsCount;
                    }
                    steps.Add(solution);
                }

                if (!ShouldContinue) break;
            }

            return steps;
        }

        protected IEnumerable<RaceSolution> Improve(RaceState problem, RaceSolution bestSolution)
        {
            var mutations = mutator.Mutate(bestSolution);
            foreach (var mutation in mutations)
            {
                if (firstMutation == null)
                    firstMutation = mutation;
                else if (mutation.Equals(firstMutation))
                    ShouldContinue = false;

                var score = Simulation(problem.MakeCopy(), mutation);
                if (score > Simulation(problem.MakeCopy(), bestSolution))
                {
                    bestSolution = mutation;
                    bestSolution.Score = score;
                    firstMutation = null;
                    yield return bestSolution;
                }
            }
        }

        private double Simulation(RaceState problem, RaceSolution solve)
        {
            var commands = solve.Accelerations;
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