using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.AsyncWorker.Interfaces;
using Game.Player.PlayerStateMashine;
using UI.Storage;
using UnityEngine;
using Zenject;

namespace Game.AsyncOperation
{
    public class AsyncWorker : IAwaiter, IInitializable
    {
        private StateMachineData _data;
        private Queue<Action> _dashAction = new();
        private readonly Queue<Action> _dashRollback = new();
        private int _maxQueueSize;
        private ValueCountStorage<int> _valueModelDash;

        public AsyncWorker(ValueCountStorage<int> valueModelDash, StateMachineData data)
        {
            _valueModelDash = valueModelDash;
            _data = data;
        }

        public async void Initialize()
        {
            await AwaitLoadConfigs(_data.PlayerConfigs);
            
            _maxQueueSize = _data.PlayerConfigs.MovementConfigsProvider.DashConfig.NumberChargesDash;

            for (var i = 0; i < _maxQueueSize; i++)
                EnqueueToQueue(() => DashOperationCount(1));

            _valueModelDash.SetValue(_maxQueueSize);

            ThreadPool.QueueUserWorkItem(async (state) => { await ProcessQueue(); });
        }

        public async UniTask AwaitLoadConfigs(ILoadable loadable)
        {
            await UniTask.WaitUntil(() => loadable.IsLoaded);
        }

        public void Dash(int sign)
        {
            DashOperationCount(sign);

            _dashRollback.Enqueue(() =>
            {
                DequeueToQueue();
                EnqueueToQueue(() => DashOperationCount(1));
            });
        }

        private void EnqueueToQueue(Action delegateToQueue)
        {
            if (_dashAction.Count < _maxQueueSize)
                _dashAction.Enqueue(delegateToQueue);
            else
                Debug.LogWarning("Очередь задач переполнена");
        }

        private void DequeueToQueue()
        {
            if (_dashAction.Count > 0)
                _dashAction.Dequeue()?.Invoke();
        }

        private void DashOperationCount(int sign)
        {
            _data.DashCount += sign;
            _valueModelDash.ChangeValue(_data.DashCount);
            _data.DashCount =
                Mathf.Clamp(_data.DashCount, 0, _data.PlayerConfigs.MovementConfigsProvider.DashConfig.NumberChargesDash);
        }

        private async UniTask ProcessQueue()
        {
            while (true)
            {
                while (_dashRollback.Count == 0)
                {
                    await UniTask.Yield();
                }

                var action = _dashRollback.Dequeue();
                await UniTask.Delay(TimeSpan.FromSeconds(_data.PlayerConfigs.MovementConfigsProvider.DashConfig.DashDelay));
                action();
            }
        }

    }
}