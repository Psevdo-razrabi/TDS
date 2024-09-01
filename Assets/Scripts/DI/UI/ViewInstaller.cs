using DI.Debugger_Remove_;
using UnityEngine;

namespace DI
{
    public class ViewInstaller : BaseBindings
    {
        [SerializeField] private DebuggerView _debugger;
        public override void InstallBindings()
        {
            BindViewDebuggable();
        }

        private void BindViewDebuggable()
        {
            BindInstance(_debugger);
        }
    }
}