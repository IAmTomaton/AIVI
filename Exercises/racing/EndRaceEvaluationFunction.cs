namespace AiAlgorithms.racing
{
    class EndRaceEvaluationFunction : IEvaluationFunction<RaceState>
    {
        public double Evaluate(RaceState state)
        {
            if (!state.Car.IsAlive) return double.NegativeInfinity;
            return 1000 - state.Time;
        }
    }
}
