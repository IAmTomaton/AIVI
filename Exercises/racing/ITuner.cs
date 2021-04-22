using System;
using System.Collections.Generic;
using System.Text;

namespace AiAlgorithms.racing
{
    interface ITuner
    {
        Dictionary<string, List<string>> Tune(int trialsCount);

    }
}
