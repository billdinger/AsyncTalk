using System;

namespace AsyncDemo
{
    class Program
    {
        private static TaskExamples Examples { get; }

        private static AsyncExamples AsyncExamples { get; }

        static Program()
        {
            Examples = new TaskExamples();
            AsyncExamples = new AsyncExamples();
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("Starting {0}", DateTime.Now);
            //var load = new Workload();
            //load.LotsOfWork();
            //Console.WriteLine("Finished {0}", DateTime.Now);

            // var result = AsyncExamples.GetAWebsiteAsync();
            //Console.WriteLine(result.Result);

            //  Examples.ReturnResultsFromMany();
            // Examples.ContinueTask();
            // Examples.ContinueTaskWithOptions();
            // Examples.RunATask();
            // Examples.RunABunchOfTasksSync();
             Examples.RunABunchOfTasks();
            // Examples.AwaitRunABunchOfTasks();
            Console.ReadLine();
        }
    }
}
