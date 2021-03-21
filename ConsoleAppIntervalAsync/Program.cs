using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppIntervalAsync
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Observable
                .Interval(TimeSpan.FromMilliseconds(1))
                .Select(l => Observable.FromAsync(ProcAsync))
                .Concat()
                .Subscribe();

            Console.ReadLine();
        }

        private static async Task ProcAsync()
        {
            Console.WriteLine($"Interval Start: {DateTime.Now:hh:mm:ss.fff}");
            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000);
            Console.WriteLine($"Interval End: {DateTime.Now:hh:mm:ss.fff}");
        }
    }
}