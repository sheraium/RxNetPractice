using System;
using System.Reactive.Linq;
using Rx.Extensions;

namespace ConsoleApp19
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var tenToTwenty = Observable.Range(10, 11);
            tenToTwenty.Inspect("range");

            var generated = Observable.Generate(
                 1,
                 value => value < 100,
                 value => value * value + 1,
                 value => $"[{value}]"); // select()
            generated.Inspect("generate");

            var interval = Observable.Interval(TimeSpan.FromMilliseconds(500));
            using (interval.Inspect("interval"))
            {
                Console.ReadKey();
            }

            var timer = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            timer.Inspect("timer");
            Console.ReadLine();
        }
    }
}