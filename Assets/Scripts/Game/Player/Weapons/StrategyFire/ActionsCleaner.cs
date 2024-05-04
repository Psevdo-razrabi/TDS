using System;
using System.Collections.Generic;

namespace Game.Player.Weapons.StrategyFire
{
    public class ActionsCleaner
    {
        private Queue<IDisposable> _disposables;
        
        public void RemoveAction()
        {
            if(_disposables.Count == 0) return;
            _disposables.Dequeue().Dispose();
        }

        public void AddAction(IDisposable disposable)
        {
            _disposables.Enqueue(disposable);
        }
    }
}