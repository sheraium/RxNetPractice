using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Timers;
using Rx.Extensions;

namespace ConsoleApp18
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var obs = Observable.Create<string>(observer =>
            {
                var timer = new Timer(1000);
                timer.Elapsed += (sender, eventArgs) => observer.OnNext($"tick {eventArgs.SignalTime}");
                timer.Elapsed += TimerOnElapsed;
                timer.Start();
                return () =>
                {
                    timer.Elapsed -= TimerOnElapsed;
                    timer.Dispose();
                };
            });

            var sub = obs.Inspect("timer");
            Console.ReadLine();

            sub.Dispose();
            Console.ReadLine();
        }

        private static void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"tock {e.SignalTime}");
        }
    }
}