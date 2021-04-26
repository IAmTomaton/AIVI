using System;
using System.Collections.Generic;
using System.Text;

namespace AiAlgorithms.racing
{
    interface ITuner<TState>
    {
        ComparisonResult Tune(IEvaluationFunction<TState> evaluationFunction, int trialsCount);
    }
}
