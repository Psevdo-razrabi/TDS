using System;
using Customs;
using Cysharp.Threading.Tasks;
using Enemy.interfaces;
using UnityEngine;

namespace Enemy
{
    public class Die<T> : IDie<T>
    {
        private readonly EventController _eventController;
        private readonly RagdollHelper _ragdollHelper;

        public Die(EventController eventController, RagdollHelper ragdollHelper)
        {
            _eventController = eventController;
            _ragdollHelper = ragdollHelper;
        }
        
        public async UniTask Died()
        {
            if (typeof(T) == typeof(Enemy))
            {
                _eventController.OnEnemyDie();
            }
            _ragdollHelper.SetActive();
            await UniTask.Delay(TimeSpan.FromMinutes(2f));
            _ragdollHelper.SetNotActive();
        }
    }
}