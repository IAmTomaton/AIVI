using System;
using System.Collections.Generic;
using AiAlgorithms.Algorithms;
using System.Linq;

namespace AiAlgorithms.racing
{
    public class HillClimbingMutator
    {
        private V[] vectors = new V[9];

        public HillClimbingMutator()
        {
            var index = 0;
            for (var dx = -1; dx < 2; dx++)
            {
                for (var dy = -1; dy < 2; dy++)
                {
                    vectors[index] = new V(dx, dy);
                    index++;
                }
            }
        }

        public IEnumerable<RaceSolution> Mutate(RaceSolution parentSolution)
        {
            foreach (var vector in vectors)
            {
                for (var i = 0; i < parentSolution.Accelerations.Length; i++)
                {
                    var oldAccelerations = new V[parentSolution.Accelerations.Length];
                    parentSolution.Accelerations.CopyTo(oldAccelerations, 0);
                    oldAccelerations[i] = vector;
                    var newRace = new RaceSolution(oldAccelerations);
                    yield return newRace;
                }
            }
        }
    }
}