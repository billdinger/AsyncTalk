using System;
using System.Collections.Generic;
using System.Threading;

namespace AsyncDemo
{
    public class Workload
    {
        private Random Rando { get; }

        public Workload()
        {
            Rando = new Random();
        }

        public void CheckIfCancel(CancellationToken ct)
        {
            while (ct.IsCancellationRequested == false)
            {
                if (ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }
                var waitTime = Rando.Next(0, 10);
                Console.WriteLine($"Not Cancelled waiting {waitTime} ");
                Thread.Sleep(Rando.Next(0, 10));
            }
        }

        public int SomeWork()
        {
            var waitTime = Rando.Next(0, 10);
            Thread.Sleep(new TimeSpan(0, 0, waitTime));
            return Rando.Next();
        }

        public IEnumerable<int> LotsOfWork()
        {
            var waitTime = Rando.Next(0, 10);
            Thread.Sleep(new TimeSpan(0, 0, waitTime));

            var list = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(Rando.Next());
            }

            return list;
        }

        public void SpinWait(int sleep)
        {
            Thread.Sleep(new TimeSpan(0, 0, sleep));
            Console.WriteLine($"I slept for {sleep} seconds");
        }

    }
}
