using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTechnologyCenter
{
    interface ISettings
    {
        TimeSpan MaxCallDuration { get; }
    }
    interface ITimeProvider
    {
        DateTime Now { get; }
    }
    interface ICallStore
    {
        ICall FindOrCreateCall(int _id);
        void CloseByTimeoutCalls();
    }
    interface ICall
    {
        DateTime CallCreateTime { get; }
        bool IsClosed { get; }
        void Close();
    }

    internal class CallStore: ICallStore
    {
        private Dictionary<int, Call> _calls = new Dictionary<int, Call>();
        private ISettings _settings;
        private ITimeProvider _timeProvider;

        public CallStore(ISettings settings, ITimeProvider timeProvider)
        {
            _settings = settings;
            _timeProvider = timeProvider;
        }

        public ICall FindOrCreateCall(int _id)
        {
            if (!_calls.TryGetValue(_id, out Call? call))
            {
                call = new Call(_timeProvider.Now );
                _calls.Add(_id, call);
            }
            return call;
        }
        public void CloseByTimeoutCalls()
        {
            foreach (var call in _calls.Values)
            {
                if (_timeProvider.Now - call.CallCreateTime > _settings.MaxCallDuration)
                    call.Close();
            }
        }

    }
    public class Call: ICall
    {
        public DateTime CallCreateTime { get; }
        public bool IsClosed { get; private set; }
        public Call(DateTime _callCreateTime)
        {
            CallCreateTime = _callCreateTime;
        }
        public void Close() {
            IsClosed = true;
            /*do something long time*/}

    }
    public class Settings:ISettings
    {
        public TimeSpan MaxCallDuration => TimeSpan.FromHours(4);
    }
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
    public class TimeProviderStub : ITimeProvider
    {
        public DateTime Now { get; set; }
    }

    public class CallStoreTests
    {
        public void CloseByTimeoutCalls_ShouldCloseCallExceedingMaxDuration()
        {
            // Arrange
            var initialTime = DateTime.Now;
            var settings = new Settings();
            var timeProvider = new TimeProviderStub() {Now = initialTime };
            var callStore = new CallStore(settings, timeProvider);
            //Act
            var call = callStore.FindOrCreateCall(0);
            timeProvider.Now= initialTime.Add(settings.MaxCallDuration)+new TimeSpan(1,0,0);
            callStore.CloseByTimeoutCalls();

            // Assert
            Assert.IsTrue(call.IsClosed);
        }
    }
}
