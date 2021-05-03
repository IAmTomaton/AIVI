using System;
using System.Collections.Generic;
using System.Text;

namespace AiAlgorithms.racing
{
    interface ITuner<TSet, TState>
    {
        ComparisonResult Tune(IEvaluationFunction<TState> evaluationFunction, List<TSet> testSet, int trialsCount, ComparisonResult initialData = null);
    }
}
