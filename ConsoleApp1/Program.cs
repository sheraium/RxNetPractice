using System;
using System.Reactive.Subjects;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var market = new Subject<float>();
            market.Subscribe(
                f => Console.WriteLine($"Market gave us {f}"),
                () => Console.WriteLine("Complete")
            );
            market.OnNext(1.24f);
            market.OnCompleted();
        }
    }
}