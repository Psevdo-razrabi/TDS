using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Weapons.StrategyFire
{
    public class ActionsCleaner
    {
        private Queue<IDisposable> _disposables = new();
        
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