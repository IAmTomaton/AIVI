namespace AiAlgorithms.racing
{
    class IntermediateRaceEF : IEvaluationFunction<RaceState>
    {
        private int flagCost;
        private int distanceCost;

        public IntermediateRaceEF(int flagCost, int distanceCost)
        {
            this.flagCost = flagCost;
            this.distanceCost = distanceCost;
        }

        public double Evaluate(RaceState state)
        {
            var car = state.Car;
            if (!car.IsAlive) return double.NegativeInfinity;
            var value = car.FlagsTaken * flagCost - car.Pos.DistTo(state.GetFlagFor(car)) * distanceCost;

            return value;
        }
    }
}
