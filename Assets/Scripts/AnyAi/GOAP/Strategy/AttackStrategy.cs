using System;
using UniRx;
using UnityEngine;

namespace GOAP
{
    public class AttackStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        public void Start()
        {
            Complete = false;
            Observable.Timer(TimeSpan.FromSeconds(1f))
                .Subscribe(_ => Attack());
        }


        private void Attack()
        {
            Debug.Log("IM ATTACK");
            Complete = true;
        }
    }
}