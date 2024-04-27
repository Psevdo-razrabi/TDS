using System;
using System.Collections.Generic;
using System.Reflection;
using Customs;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.Mediators;
using Game.Player.Weapons.StrategyFire;
using Input.Interface;
using UniRx;
using UnityEngine;
using Zenject;

namespace Input
{
    public class ChangeModeFire : MonoBehaviour, ISetFireModes
    {
        public string fireMode;
        private InputSystem _inputSystem;
        private readonly Queue<MethodInfo> _queueStates = new();
        private readonly Subject<Unit> _dashClick = new();
        private readonly CompositeDisposable _compositeDisposable = new();
        private MethodInfo _modeFire;
        private MediatorFireStrategy _fireStrategy;
        
        private void OnEnable()
        {
            _inputSystem.Enable();
            _inputSystem.Weapon.ChangeFireMode.performed +=_ => _dashClick.OnNext(Unit.Default);

            _dashClick.ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(async _ => await ChangeMode())
                .AddTo(_compositeDisposable);   //условно задержка без переменной
        }
        
        [ContextMenuAttribute("single fire")]
        private void AddSingleFire()
        {
            _fireStrategy.ChangeFireMode(new SingleFire());
        }
        
        [ContextMenuAttribute("burst fire")]
        private void AddBurstFire()
        {
            _fireStrategy.ChangeFireMode(new BurstFire());
        }
        
        [ContextMenuAttribute("automatic fire")]
        private void AddAutomaticFire()
        {
            _fireStrategy.ChangeFireMode(new AutomaticFire());
        }
        
        public void SetFireModes(List<MethodInfo> methodFireStates)
        {
            methodFireStates.ForEach(x => _queueStates.Enqueue(x));
        }

        private async UniTask ChangeMode()
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
        private void Construct(InputSystem inputSystem, MediatorFireStrategy fireStrategy)
        {
            _inputSystem = inputSystem;
            _fireStrategy = fireStrategy;
        }
        
        private void OnDisable()
        {
            _inputSystem.Disable();
            _inputSystem.Dispose();
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }
    }
}