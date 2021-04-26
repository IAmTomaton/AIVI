namespace AiAlgorithms.racing
{
    interface IEvaluationFunction<TState>
    {
        public double Evaluate(TState state);
    }
}
