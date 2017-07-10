using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace AsyncDemo
{
    public class ParallelExamples
    {
        private Workload Load { get; }

        public ParallelExamples()
        {
            Load = new Workload();
        }

        public void ASimpleExample()
        {
            var result =
            (from z in Load.LotsOfWork().AsParallel()
             select z).Max();
            Console.WriteLine(result);
        }

        public void PlinqExample()
        {
            var range = Enumerable.Range(1, 10000);
            var avg =
                (from x in range.AsParallel() select x).Average();
            Console.WriteLine(avg);
        }

        public void DegreeOfParallelismExample()
        {
            var range = Enumerable.Range(1, 10000);
            var avg =
                (from x in
                     range.AsParallel().WithDegreeOfParallelism(10)
                 select x).Average();
            Console.WriteLine(avg);
        }

        public void CancellationExample()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var range = Enumerable.Range(1, 10000);
            var avg =
                (from x in
                     range.AsParallel().WithCancellation(token)
                 select x).Average();
            cts.Cancel();
            Console.WriteLine(avg);
        }

        public void ForAllExample()
        {
            var range = Enumerable.Range(1, 10000);
            var query = from num in range.AsParallel()
                        where num % 10 == 0
                        select num;

            var bag = new ConcurrentBag<int>();
            query.ForAll(e => bag.Add(e));
        }
    }
}
