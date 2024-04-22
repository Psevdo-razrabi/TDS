using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Zenject;

namespace Game.AsyncWorker
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class AsyncWorker
    {
        private IStateDataWorker _dataWorkerStateMachine;
        private Queue<Action> _queueTask = new();
        private int _maxQueueSize;
        private bool _isTaskComplite;

        [Inject]
        public async void Construct(IStateDataWorker dataWorker)
        {
            _dataWorkerStateMachine = dataWorker;
            await Await(_dataWorkerStateMachine.PlayerConfigs);

            _maxQueueSize = _dataWorkerStateMachine.PlayerConfigs.DashConfig.NumberChargesDash;

            for (var i = 0; i < _maxQueueSize; i++)
                EnqueueToQueue(() => DashOperationCount(1));
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
            _dataWorkerStateMachine.Text1.Value = _dataWorkerStateMachine.StateMachineData.DashCount;
            _dataWorkerStateMachine.StateMachineData.DashCount =
                Mathf.Clamp(_dataWorkerStateMachine.StateMachineData.DashCount, 0, _dataWorkerStateMachine.PlayerConfigs.DashConfig.NumberChargesDash);
            Debug.Log(_dataWorkerStateMachine.StateMachineData.DashCount);
        }
        
        public async UniTask Await(PlayerConfigs configs)
        {
            while (!configs.IsLoadDashConfig)
                await UniTask.Yield();
        }

        public async UniTask Await()
        {
            while (!_isTaskComplite)
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