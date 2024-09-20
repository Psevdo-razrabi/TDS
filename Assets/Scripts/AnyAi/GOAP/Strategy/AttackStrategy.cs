using System;
using System.Threading;
using UniRx;
using UnityEngine;

namespace GOAP
{
    public class AttackStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; } = null;

        private IDisposable _disposable;

        public void Start()
        {
            Complete = false;
            _disposable = Observable.Timer(TimeSpan.FromSeconds(1f))
                .Subscribe(_ => Attack());
        }

        public void Stop()
        {
            _disposable.Dispose();
        }

        private void Attack()
        {
            Debug.Log("IM ATTACK");
            Complete = true;
        }
    }
}