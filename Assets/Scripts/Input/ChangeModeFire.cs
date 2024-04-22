using System;
using System.Collections.Generic;
using System.Linq;
using Customs;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.Mediators;
using Game.Player.Weapons.StrategyFire;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Input
{
    public class ChangeModeFire : MonoBehaviour
    {
        public A[] mode;
        private InputSystem _inputSystem;
        private Queue<Action> _queueStates;
        private Subject<Unit> _dashClick = new();
        private CompositeDisposable _compositeDisposable = new();
        private Action _modeFire;
        private MediatorFireStrategy _fireStrategy;
        private MethodList _methodList;

        [Inject]
        public void Construct(InputSystem inputSystem, MediatorFireStrategy fireStrategy)
        {
            _inputSystem = inputSystem;
            _fireStrategy = fireStrategy;
            //_methodList = methodList;
        }
        
        private void OnEnable()
        {
            _inputSystem.Enable();
            _inputSystem.Weapon.ChangeFireMode.performed +=_ => _dashClick.OnNext(Unit.Default);

            _dashClick.ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(async _ => await ChangeMode())
                .AddTo(_compositeDisposable);   //условно задержка без переменной
        }
        
        [ContextMenuAttribute("single fire")]
        public void AddSingleFire()
        {
            _fireStrategy.ChangeFireMode(new SingleFire());
        }
        
        [ContextMenuAttribute("burst fire")]
        public void AddBurstFire()
        {
            _fireStrategy.ChangeFireMode(new BurstFire());
        }
        
        [ContextMenuAttribute("automatic fire")]
        public void AddAutomaticFire()
        {
            _fireStrategy.ChangeFireMode(new AutomaticFire());
        }

        // private void GetMethods()
        // {
        //     _methodList.methods = _methodNameList.methodNames.Select(name =>
        //     {
        //         Type targetType = _methodList.gameObject.GetComponent<MonoBehaviour>().GetType();
        //         
        //         return targetType.GetMethod(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        //     })
        //         .ToList();
        // }

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
            _modeFire();
            _modeFire = null;
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
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