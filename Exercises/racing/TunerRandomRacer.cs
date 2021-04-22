using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AiAlgorithms.racing
{
    class TunerRandomRacer : ITuner
    {
        public Dictionary<string, List<string>> Tune(int trialsCount)
        {
            var tasks = new List<Task<Dictionary<string, string>>>();

            for (var maxDepth = 10; maxDepth <= 10; maxDepth++)
            {
                for (var depthDivider = 4; depthDivider <= 4; depthDivider++)
                {
                    for (var minDepth = 5; minDepth <= 5; minDepth++)
                    {
                        var tempMaxDepth = maxDepth;
                        var tempDepthDivider = depthDivider;
                        var tempMinDepth = minDepth;
                        var task = Task.Run(() => DoTrial(trialsCount, tempMaxDepth, tempDepthDivider, tempMinDepth));
                        tasks.Add(task);
                    }
                }
            }

            var results = new Dictionary<string, List<string>>
            {
                { "maxDepth", new List<string>() },
                { "depthDivider", new List<string>() },
                { "minDepth", new List<string>() },
                { "mate", new List<string>() },
                { "dispersion", new List<string>() }
            };

            foreach (var task in tasks)
            {
                task.Wait();
                var result = task.Result;
                foreach (var key in result.Keys)
                {
                    results[key].Add(result[key]);
                }
            }

            return results;
        }

        private Dictionary<string, string> DoTrial(int trialsCount, int maxDepth, int depthDivider, int minDepth)
        {
            var results = new Dictionary<string, string>();
            (double mateExpectation, double dispersion) = StatisticsСollector.Test(trialsCount, () =>
            {
                var racer = new RandomRacer(maxDepth, depthDivider, minDepth);
                var test = RaceProblemsRepo.GetTests(false).ElementAt(0);
                var result = RaceController.Play(test, racer, false);
                return result.Time;
            });
            results["maxDepth"] = maxDepth.ToString();
            results["depthDivider"] = depthDivider.ToString();
            results["minDepth"] = minDepth.ToString();
            results["mate"] = mateExpectation.ToString();
            results["dispersion"] = dispersion.ToString();

            return results;
        }
    }
}
