using System;
using System.Reactive.Subjects;

namespace ConsoleApp2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sensor = new Subject<float>();
            using (sensor.Subscribe(Console.WriteLine))
            {
                sensor.OnNext(1, 2, 3);
            }

            sensor.OnNext(4);
        }
    }

    internal static class SubjectExtension
    {
        public static void OnNext<T>(this Subject<T> subject, params T[] values)
        {
            foreach (var value in values)
            {
                subject.OnNext(value);
            }
        }
    }
}