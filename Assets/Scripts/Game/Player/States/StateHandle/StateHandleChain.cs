using System.Collections.Generic;
using System.Linq;

namespace Game.Player.States.StateHandle
{
    public class StateHandleChain
    {
        private List<IStateHandle> _handles;

        public StateHandleChain(List<IStateHandle> handles)
        {
            _handles = handles;
        }

        public void HandleState() => _handles
            .FirstOrDefault(handle => handle.CanHandle())?
            .Handle();

        public void HandleState<T>() =>
            _handles
                .FirstOrDefault(handle => handle.GetType() == typeof(T) && handle.CanHandle())?
                .Handle();

        public bool CanHandleState<T>()
        {
            var handle = _handles.FirstOrDefault(handle => handle.GetType() == typeof(T) && handle.CanHandle());
            return handle != null;
        }
    }
}