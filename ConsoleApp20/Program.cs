using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Rx.Extensions;

namespace ConsoleApp20
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Start();
            //FromEvent();
            //FromTask();
            FromList();
        }

        private static void FromList()
        {
            var item = new List<int>() { 1, 2, 3 };
            var source = item.ToObservable();
            source.Inspect("observable");
        }

        private static void FromTask()
        {
            var t = Task.Factory.StartNew(() => "Test");
            var source = t.ToObservable();
            source.Inspect("task");

            Console.ReadLine();
        }

        private static void FromEvent()
        {
            var market = new Market();
            var priceChanges = Observable.FromEventPattern<float>(
                handler => market.PriceChanged += handler,
                handler => market.PriceChanged -= handler
            );

            priceChanges.Subscribe(
                x => Console.WriteLine($"{x.EventArgs}"));

            market.ChangePrice(1);
            market.ChangePrice(1.2f);
            market.ChangePrice(1.2f);
        }

        private static void Start()
        {
            var start = Observable.Start(() =>
            {
                Console.WriteLine("Starting work...");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(200);
                    Console.Write(".");
                }
            });

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.Write("-");
            }

            start.Inspect("start");
            Console.ReadLine();
        }
    }

    internal class Market
    {
        private float _price;

        public float Price
        {
            get => _price;
            set => _price = value;
        }

        public event EventHandler<float> PriceChanged;

        public void ChangePrice(float price)
        {
            _price = price;
            PriceChanged?.Invoke(this, price);
        }
    }
}