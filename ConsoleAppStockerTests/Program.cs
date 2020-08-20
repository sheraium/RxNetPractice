using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppStockerTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var stocker = new Stocker();
            Console.WriteLine("Start" + Thread.CurrentThread.ManagedThreadId);

            var task = Task.Run(() =>
            {
                var job = stocker.Transfer();
                job
                    //.ObserveOn(Scheduler.Default)
                    //.ObserveOn(Scheduler.CurrentThread)
                    //.ObserveOn(NewThreadScheduler.Default)
                    //.ObserveOn(TaskPoolScheduler.Default)
                    //.SubscribeOn(TaskPoolScheduler.Default)
                    //.SubscribeOn(Scheduler.CurrentThread)
                    .Subscribe(jobInfo => { Console.WriteLine(jobInfo.ToString()); }
                        ,
                        e => { Console.WriteLine($"Exception: {e.Message} {Thread.CurrentThread.ManagedThreadId}"); }
                        ,
                        () =>
                        {
                            //isComplete = true;
                        }
                    );
            });

            task.Wait();
            Console.WriteLine("Complete" + Thread.CurrentThread.ManagedThreadId);
        }
    }

    internal class Stocker
    {
        private Crane _crane;

        public Stocker()
        {
            _crane = new Crane();
        }

        public IObservable<JobInfo> Transfer()
        {
            // 前置工作，另一Crane退避
            //return _crane.Transfer();

            // 攔截
            return Observable.Create<JobInfo>(observer =>
            {
                _crane.Transfer()
                    .Subscribe(jobInfo =>
                        {
                            Console.WriteLine($"Stocker: {jobInfo}, Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                            observer.OnNext(jobInfo);
                        }
                        ,
                        e => { Console.WriteLine($"Exception: {e.Message} {Thread.CurrentThread.ManagedThreadId}"); }
                        ,
                        () =>
                        {
                            //observer.OnCompleted();
                        }
                    );

                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
    }

    internal class Crane
    {
        public IObservable<JobInfo> Transfer()
        {
            return Observable.Create<JobInfo>(observer =>
            {
                Console.WriteLine("Crane Initiated" + Thread.CurrentThread.ManagedThreadId);
                observer.OnNext(new JobInfo()
                {
                    State = CommandState.Initiated,
                    ReturnCode = 0,
                    BcrResult = String.Empty,
                    CarrierLocation = "Source",
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                });

                Thread.Sleep(1000);

                Console.WriteLine("Crane Active" + Thread.CurrentThread.ManagedThreadId);
                observer.OnNext(new JobInfo()
                {
                    State = CommandState.CraneActive,
                    ReturnCode = 0,
                    BcrResult = String.Empty,
                    CarrierLocation = "Source",
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                });

                Thread.Sleep(1000);

                Console.WriteLine("Crane Transferring" + Thread.CurrentThread.ManagedThreadId);
                observer.OnNext(new JobInfo()
                {
                    State = CommandState.Transferring,
                    ReturnCode = 0,
                    BcrResult = "CST001",
                    CarrierLocation = "Crane",
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                });

                Thread.Sleep(1000);

                Console.WriteLine("Crane Idle" + Thread.CurrentThread.ManagedThreadId);
                observer.OnNext(new JobInfo()
                {
                    State = CommandState.CraneIdle,
                    ReturnCode = 0,
                    BcrResult = "CST001",
                    CarrierLocation = "Destination",
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                });

                Thread.Sleep(1000);

                Console.WriteLine("Crane Completed" + Thread.CurrentThread.ManagedThreadId);
                observer.OnNext(new JobInfo()
                {
                    State = CommandState.Completed,
                    ReturnCode = 92,
                    BcrResult = "CST001",
                    CarrierLocation = "Destination",
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                });

                Thread.Sleep(1000);

                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
    }

    internal class JobInfo
    {
        public CommandState State { get; set; }
        public int ReturnCode { get; set; }
        public string BcrResult { get; set; }
        public string CarrierLocation { get; set; }
        public int ThreadId { get; set; }

        public override string ToString()
        {
            return $"{nameof(State)}: {State}, {nameof(ReturnCode)}: {ReturnCode}, {nameof(BcrResult)}: {BcrResult}, {nameof(CarrierLocation)}: {CarrierLocation}, {nameof(ThreadId)}: {ThreadId}";
        }
    }

    internal enum CommandState
    {
        Initiated,
        Transferring,
        CraneActive,
        CraneIdle,
        Completed,
    }
}