using System;
using System.Collections.Generic;
using System.Reflection;
using Customs;
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
        public string fireMode;
        private readonly Queue<Action> _queueStates = new();
        private Action _modeFire;
        private MediatorFireStrategy _fireStrategy;
        private FireComponent _fireComponent;
        
        [ContextMenuAttribute("single fire")]
        private void AddSingleFire()
        {
            _fireStrategy.ChangeFireMode(new SingleFire(_fireComponent));
        }
        
        [ContextMenuAttribute("burst fire")]
        private void AddBurstFire()
        {
            _fireStrategy.ChangeFireMode(new BurstFire(_fireComponent));
        }
        
        [ContextMenuAttribute("automatic fire")]
        private void AddAutomaticFire()
        {
            _fireStrategy.ChangeFireMode(new AutomaticFire(_fireComponent));
        }
        
        public void SetFireModes(List<MethodInfo> methodFireStates)
        {
            //methodFireStates.ForEach(x => _queueStates.Enqueue(x));
        }

        public async UniTask ChangeMode() {
            if (_queueStates.Count == 0)
            {
                Debug.Log("очередь пуста, что то не так");
                return;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            _modeFire = _queueStates.Dequeue();
            _queueStates.Enqueue(_modeFire);
            _modeFire();
            _modeFire = null;
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        }
        
        [Inject]
        private void Construct(MediatorFireStrategy fireStrategy, FireComponent fireComponent)
        {
            _fireStrategy = fireStrategy;
            _fireComponent = fireComponent;
            
            _queueStates.Enqueue(AddSingleFire);
            _queueStates.Enqueue(AddBurstFire);
            _queueStates.Enqueue(AddAutomaticFire);
        }
    }
}