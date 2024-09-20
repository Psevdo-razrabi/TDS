using System;
using UniRx;
using Zenject;

namespace BlackboardScripts
{
    public class BlackboardController : IDisposable
    {
        private Blackboard _blackboard;
        private Arbiter _arbiter;
        private IDisposable _disposable;
        
        public void Initialize()
        {
            _blackboard = new Blackboard();
            _arbiter = new Arbiter();
            SubscribeUpdate();
        }

        public Blackboard GetBlackboard() => _blackboard;
        public void RegisterExpert(IExpert expert) => _arbiter.Register(expert);
        public void RemoveExpert(IExpert expert) => _arbiter.Remove(expert);
        
        public T GetValue<T>(string key)
        {
            _blackboard.TryGetValue<T>(_blackboard.GetOrRegisterKey(key), out var value);
            return value;
        }

        public void SetValue<T>(string key, T value)
        {
            _blackboard.SetValue(_blackboard.GetOrRegisterKey(key), value);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
        
        private void SubscribeUpdate()
        {
            _disposable = Observable.EveryUpdate().Subscribe(_ =>
            {
                foreach (var action in _arbiter.BlackboardIteration(_blackboard))
                {
                    action();
                }
            });
        }
    }
}