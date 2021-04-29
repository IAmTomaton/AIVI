namespace AiAlgorithms.racing
{
    class EndRaceEvaluationFunction : IEvaluationFunction<RaceState>
    {
        private double timeCost;
        private double flagCost;

        public EndRaceEvaluationFunction(double timeCost = 1, double flagCost = 100)
        {
            this.timeCost = timeCost;
            this.flagCost = flagCost;
        }

        public double Evaluate(RaceState state)
        {
            if (!state.Car.IsAlive) return double.NegativeInfinity;
            return state.Car.FlagsTaken * flagCost - state.Time * timeCost;
        }
    }
}
