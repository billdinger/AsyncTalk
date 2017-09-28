using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public class TaskExamples
    {
        private Workload Load { get; }

        public TaskExamples()
        {
            Load = new Workload();
        }

        public void StartATask()
        {
            int i = 0;
            var aTask = new Task(() =>
            {
                i = 17 + 23;
            });
            aTask.Start();
            Console.WriteLine(i);
        }
        public void RunATask()
        {
            Task.Run(() => { Console.WriteLine("I'm a task!"); });
        }

        public void ReturnAValue()
        {

            var someTask = Task.Run(() => { return "a string"; });
            Console.WriteLine(someTask.Result);
        }

        public void RunABunchOfTasksSync()
        {
            var watch = Stopwatch.StartNew();
            Load.SpinWait(11);
            Load.SpinWait(4);
            Load.SpinWait(22);
            Load.SpinWait(7);
            watch.Stop();
            Console.WriteLine($"Finished in {watch.Elapsed.Seconds} seconds");
        }

        public void RunABunchOfTasks()
        {
            var watch = Stopwatch.StartNew();
            var taskA = Task.Factory.StartNew(() => Load.SpinWait(11));
            var taskB = Task.Factory.StartNew(() => Load.SpinWait(4));
            var taskC = Task.Factory.StartNew(() => Load.SpinWait(22));
            var taskD = Task.Factory.StartNew(() => Load.SpinWait(7));
            var tasks = new[] { taskA, taskB, taskC, taskD };
            Task.WaitAll(tasks);
            watch.Stop();
            Console.WriteLine($"Finished in {watch.Elapsed.Seconds} seconds");
        }

        public async Task AwaitRunABunchOfTasks()
        {
            var watch = Stopwatch.StartNew();
            var taskA = Task.Factory.StartNew(() => Load.SpinWait(11));
            var taskB = Task.Factory.StartNew(() => Load.SpinWait(4));
            var taskC = Task.Factory.StartNew(() => Load.SpinWait(22));
            var taskD = Task.Factory.StartNew(() => Load.SpinWait(7));
            var tasks = new[] { taskA, taskB, taskC, taskD };
            await Task.WhenAll(tasks);
            watch.Stop();
            Console.WriteLine($"Finished in {watch.Elapsed.Seconds}");
        }

        public void ReturnResultsFromMany()
        {
            var taskA = Task.Factory.StartNew(Load.SomeWork);
            var taskB = Task.Factory.StartNew(Load.SomeWork);
            var taskC = Task.Factory.StartNew(Load.SomeWork);
            var taskD = Task.Factory.StartNew(Load.SomeWork);

            var tasks = new List<Task<int>>() { taskA, taskB, taskC, taskD };
            while (tasks.Count > 0)
            {
                var completedIndex = Task.WaitAny(tasks.ToArray());
                var completedTask = tasks[completedIndex];
                Console.WriteLine(completedTask.Result);
                tasks.RemoveAt(completedIndex);
            }
        }

        public void CancelOneTask()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var taskA = Task.Factory.StartNew(Load.SomeWork, cts.Token);

        }

        public void CancelManyTasks()
        {
            var cts = new CancellationTokenSource();
            var taskA = Task.Factory.StartNew(() => Load.CheckIfCancel(cts.Token), cts.Token);
            var taskB = Task.Factory.StartNew(Load.SomeWork, cts.Token);
            cts.Cancel();
            Task.WaitAll(new Task[] { taskA, taskB });
        }

        public void RunATaskFactory()
        {
            var someTask =
                Task.Factory.StartNew(
                    () => { return "I'm a task factory!"; });
            Console.WriteLine(someTask.Result);
        }

        public void UnsafeClosures()
        {
            int i = 0;
            var taskA = new Task(() => i = 17 + 100);
            var taskB = new Task(() => i = 100 + 100);
            taskA.Start();
            taskB.Start();
            Console.WriteLine(i);
        }

        public void ReturnTask()
        {
            var taskA = new Task<int>(() => 17 + 100);
            var taskB = new Task<int>(() => 100 + 100);
            taskA.Start();
            taskB.Start();
            Console.WriteLine($"TaskA: {taskA} ");
            Console.WriteLine($"TaskB: {taskB} ");
        }

        public void ContinueTask()
        {
            var task =
                Task.Factory.StartNew(() => 100 + 30)
                .ContinueWith(antecedent => antecedent.Result + 17);
            Console.WriteLine(task.Result);
        }

        public void ContinueTaskWithOptions()
        {
            Task.Factory.StartNew(() => throw new Exception("Stuff"))
                .ContinueWith(
                antecedent =>
                Console.WriteLine(antecedent.Exception.Flatten().InnerException.Message),
                TaskContinuationOptions.OnlyOnFaulted);
        }

        public void UnhandledExceptions()
        {
            var taskA =
                Task.Factory.StartNew(() => throw new Exception("Stuff"));
            var taskB =
                Task.Factory.StartNew(() => throw new Exception("new"));
            var tasks = new[] { taskA, taskB };

            // will bubble up exception here:
            Task.WaitAll(tasks);
        }

        public void HandledExceptions()
        {
            var taskA =
                Task.Factory.StartNew(() => throw new Exception("Stuff"));
            var taskB =
                Task.Factory.StartNew(() => throw new Exception("new"));
            var tasks = new[] { taskA, taskB };

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException agEx)
            {

                agEx.Flatten();
                foreach (var ex in agEx.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}
