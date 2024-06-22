using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons;
using Game.Player.Weapons.Mediators;
using Game.Player.Weapons.StrategyFire;
using Input.Interface;
using UnityEngine;
using Zenject;

namespace Input
{
    public class ChangeModeFire : MonoBehaviour, ISetFireModes
    {
        private readonly Queue<MethodInfo> _queueStates = new();
        private MethodInfo _modeFire;
        private MediatorFireStrategy _fireStrategy;
        private FireComponent _fireComponent;
        
        [Customs.ContextMenu("single fire")]
        private void AddSingleFire()
        {
            _fireStrategy.ChangeFireMode(new SingleFire(_fireComponent));
        }
        
        [Customs.ContextMenu("burst fire")]
        private void AddBurstFire()
        {
            _fireStrategy.ChangeFireMode(new BurstFire(_fireComponent));
        }
        
        [Customs.ContextMenu("automatic fire")]
        private void AddAutomaticFire()
        {
            _fireStrategy.ChangeFireMode(new AutomaticFire(_fireComponent));
        }
        
        public void SetFireModes(List<MethodInfo> methodFireStates)
        {
            _queueStates.Clear();
            methodFireStates.ForEach(x => _queueStates.Enqueue(x));
            SetFireMode();
        }

        public async UniTask ChangeMode() {
            if (_queueStates.Count == 0)
            {
                Debug.Log("очередь пуста, что то не так");
                return;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            SetFireMode();
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        }
        
        [Inject]
        private void Construct(MediatorFireStrategy fireStrategy, FireComponent fireComponent)
        {
            _fireStrategy = fireStrategy;
            _fireComponent = fireComponent;
        }

        private void SetFireMode()
        {
            _modeFire = _queueStates.Dequeue();
            _queueStates.Enqueue(_modeFire);
            _modeFire.Invoke(this, null);
            _modeFire = null;
        }
    }
}