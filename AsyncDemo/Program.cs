using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var result = AsyncExamples.GetAWebsiteAsync();
            Console.WriteLine(result.Result);
            // Examples.ReturnResultsFromMany();
            // Examples.ContinueTask();
            // Examples.ContinueTaskWithOptions();
            // Examples.RunATask();
            // Examples.RunABunchOfTasksSync();
            // Examples.RunABunchOfTasks();
            // Examples.AwaitRunABunchOfTasks();
            Console.ReadLine();
        }
    }
}
