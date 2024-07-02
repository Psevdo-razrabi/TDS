using System;
using Game.Player.Weapons.InterfaceWeapon;
using UniRx;

namespace Game.Player.Weapons.Decorator
{
    public class HandlerDecoratorActions
    {
        private IAction _action;
        private Func<bool> _handlerOperation;

        public HandlerDecoratorActions(Func<bool> handlerOperation, IAction action)
        {
            _handlerOperation = handlerOperation;
            _action = action;
        }

        public bool CanExecute()
        {
            var isOperationToExecute = _handlerOperation();
            if(isOperationToExecute) _action.Execute();
            return isOperationToExecute;
        }
    }
}