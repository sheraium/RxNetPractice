using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class ABC
    {
        public string Name { get; set; } = "ABC";
    }

    internal class Program
    {
        private static int count = 0;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            NewMethod();

            //var list = new List<ABC>();
            //while (true)
            //{
            //    list.Add(new ABC());
            //    await Task.Delay(10);
            //}

            Console.ReadLine();
        }

        private static void NewMethod()
        {
            var a = Observable.Interval(TimeSpan.FromMilliseconds(100))
                  .Select(l => Observable.FromAsync(DoSomething))
                  .Concat()
                  .Subscribe();

            Console.ReadLine();
            a.Dispose();
        }

        private static async Task DoSomething()
        {
            count++;
            //if (count > 100)
            //{
            //    count = 0;
            //    GC.Collect();
            //    Console.WriteLine($"Memory used before collection:  {GC.GetTotalMemory(false):N0}");
            //}
            if (count < 10)
            {
                await Task.Delay(3000);
            }
            Console.WriteLine("DoSomething");
        }
    }
}