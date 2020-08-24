using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ConsoleAppStockerUsingAsync
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var stocker = new Stocker();

            var disposable = stocker.CommandEvents
                .Subscribe(s =>
                {
                    Console.WriteLine(s);
                });

            await stocker.Transfer("CmdId001");
            disposable.Dispose();
        }
    }

    internal class Stocker
    {
        private Crane _crane;
        public IObservable<string> CommandEvents => _commandEventSubject;
        private ISubject<string> _commandEventSubject = new Subject<string>();

        public Stocker()
        {
            _crane = new Crane(this);
        }

        public async Task Transfer(string cmdId)
        {
            var disposable = _crane.Events
                .Subscribe(s =>
                {
                    _commandEventSubject.OnNext($"{cmdId} {s}");
                });

            await _crane.Move("JobId001");
            await Task.Delay(200);
            await _crane.PickUp("JobId002");
            await Task.Delay(200);
            await _crane.Move("JobId003");
            await Task.Delay(200);
            await _crane.Deposit("JobId004");

            disposable.Dispose();
        }

        public async Task Transfer1()
        {
            await Task.Delay(200);
            await _crane.Transfer("JobId100");
        }
    }

    internal class Crane
    {
        private readonly Stocker _stocker;
        public IObservable<string> Events => _eventSubject;
        private ISubject<string> _eventSubject = new Subject<string>();

        public Crane(Stocker stocker)
        {
            _stocker = stocker;
        }

        public async Task Move(string jobId)
        {
            await Task.Run(() =>
            {
                _eventSubject.OnNext($"{jobId} : Crane Active");
                _eventSubject.OnNext($"{jobId} : Crane Idle");
            });
        }

        public async Task PickUp(string jobId)
        {
            await Task.Run(() =>
            {
                _eventSubject.OnNext($"{jobId} : Crane Active");
                _eventSubject.OnNext($"{jobId} : Crane PickUp");
                _eventSubject.OnNext($"{jobId} : Crane Idle");
            });
        }

        public async Task Deposit(string jobId)
        {
            await Task.Run(() =>
            {
                _eventSubject.OnNext($"{jobId} : Crane Active");
                _eventSubject.OnNext($"{jobId} : Crane Deposit");
                _eventSubject.OnNext($"{jobId} : Crane Idle");
            });
        }

        public async Task Transfer(string jobId)
        {
            await Task.Run(() =>
            {
                _eventSubject.OnNext($"{jobId} : Crane Active");
                _eventSubject.OnNext($"{jobId} : Crane PickUp");
                _eventSubject.OnNext($"{jobId} : Crane Deposit");
                _eventSubject.OnNext($"{jobId} : Crane Idle");
            });
        }
    }
}