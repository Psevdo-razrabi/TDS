using System;
using System.Collections.Generic;
using System.Reflection;
using Customs;
using Cysharp.Threading.Tasks;
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
        private readonly Queue<MethodInfo> _queueStates = new();
        private MethodInfo _modeFire;
        private MediatorFireStrategy _fireStrategy;
        private InputSystemWeapon _inputSystemWeapon;
        private MouseInputObserver _mouseInputObserver;
        
        [ContextMenuAttribute("single fire")]
        private void AddSingleFire()
        {
            _fireStrategy.ChangeFireMode(new SingleFire(_inputSystemWeapon, _mouseInputObserver));
        }
        
        [ContextMenuAttribute("burst fire")]
        private void AddBurstFire()
        {
            _fireStrategy.ChangeFireMode(new BurstFire(_inputSystemWeapon, _mouseInputObserver));
        }
        
        [ContextMenuAttribute("automatic fire")]
        private void AddAutomaticFire()
        {
            _fireStrategy.ChangeFireMode(new AutomaticFire(_inputSystemWeapon, _mouseInputObserver));
        }
        
        public void SetFireModes(List<MethodInfo> methodFireStates)
        {
            methodFireStates.ForEach(x => _queueStates.Enqueue(x));
        }

        public async UniTask ChangeMode()
        {
            if (_queueStates.Count == 0)
            {
                Debug.Log("очередь пуста, что то не так");
                return;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            _modeFire = _queueStates.Dequeue();
            _queueStates.Enqueue(_modeFire);
            _modeFire.Invoke(this, null);
            _modeFire = null;
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        }
        
        [Inject]
        private void Construct(MediatorFireStrategy fireStrategy, InputSystemWeapon inputSystemWeapon, MouseInputObserver mouseInputObserver)
        {
            _fireStrategy = fireStrategy;
            _inputSystemWeapon = inputSystemWeapon;
            _mouseInputObserver = mouseInputObserver;
        }
    }
}