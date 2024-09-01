using Zenject;

namespace BlackboardScripts
{
    public class BlackboardController : ITickable
    {
        private readonly Blackboard _blackboard;
        private readonly Arbiter _arbiter;

        public BlackboardController(Blackboard blackboard, Arbiter arbiter)
        {
            _blackboard = blackboard;
            _arbiter = arbiter;
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
        
        public void Tick()
        {
            foreach (var action in _arbiter.BlackboardIteration(_blackboard))
            {
                action();
            }
        }
    }
}