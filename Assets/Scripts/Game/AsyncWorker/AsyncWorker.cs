using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Game.Player.Weapons.WeaponConfigs;
using Zenject;

namespace Game.AsyncWorker
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class AsyncWorker : IInitializable
    {
        private IStateDataWorker _dataWorkerStateMachine;
        private Queue<Action> _queueTask = new();
        private int _maxQueueSize;
        private bool _isTaskComplite;
        
        public async void Initialize()
        {
            await Await(_dataWorkerStateMachine.PlayerConfigs);

            _maxQueueSize = _dataWorkerStateMachine.PlayerConfigs.DashConfig.NumberChargesDash;

            for (var i = 0; i < _maxQueueSize; i++)
                EnqueueToQueue(() => DashOperationCount(1));
            
            _dataWorkerStateMachine.ValueModelDash.SetValue(_maxQueueSize);
        }

        private void EnqueueToQueue(Action delegateToQueue)
        {
            if (_queueTask.Count < _maxQueueSize)
                _queueTask.Enqueue(delegateToQueue);
            else
                Debug.LogWarning("Очередь задач переполнена");
        }

        private async UniTask DequeueToQueue()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_dataWorkerStateMachine.PlayerConfigs.DashConfig.DashDelay));

            if (_queueTask.Count > 0)
                _queueTask.Dequeue()?.Invoke();
            
            _isTaskComplite = true;
        }

        private void DashOperationCount(int sign)
        {
            _dataWorkerStateMachine.StateMachineData.DashCount += sign;
            _dataWorkerStateMachine.ValueModelDash.ChangeValue(_dataWorkerStateMachine.StateMachineData.DashCount);
            _dataWorkerStateMachine.StateMachineData.DashCount =
                Mathf.Clamp(_dataWorkerStateMachine.StateMachineData.DashCount, 0, _dataWorkerStateMachine.PlayerConfigs.DashConfig.NumberChargesDash);
            Debug.Log(_dataWorkerStateMachine.StateMachineData.DashCount);
        }
        
        private async UniTask Await()
        {
            while (!_isTaskComplite)
                await UniTask.Yield();
        }
        
        [Inject]
        private void Construct(IStateDataWorker dataWorker, WeaponData weaponData)
        {
            _dataWorkerStateMachine = dataWorker;
        }
        
        public async UniTask Await(PlayerConfigs configs)
        {
            while (!configs.IsLoadAllConfig)
                await UniTask.Yield();
        }

        public async UniTask Dash(int sign)
        {
            DashOperationCount(sign);
            _isTaskComplite = false;
            await UniTask.SwitchToThreadPool(); 
            await DequeueToQueue();
            await Await();
            await UniTask.SwitchToMainThread();
            EnqueueToQueue(() => DashOperationCount(1));
        }
    }
}