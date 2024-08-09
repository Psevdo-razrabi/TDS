using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Game.Player.Weapons.Prefabs;
using Game.Player.Weapons.WeaponConfigs;
using UI.Storage;
using UnityEngine;
using Zenject;

namespace Game.AsyncWorker.Interfaces
{
    public class AsyncWorker : IInitializable, IAwaiter
    {
        private IStateDataWorker _dataWorkerStateMachine;
        private Queue<Action> _dashAction = new();
        private readonly Queue<Action> _dashRollback = new();
        private int _maxQueueSize;
        private ValueCountStorage<int> _valueModelDash;

        public async void Initialize()
        {
            await AwaitLoadPlayerConfig(_dataWorkerStateMachine.PlayerConfigs);

            _maxQueueSize = _dataWorkerStateMachine.PlayerConfigs.DashConfig.NumberChargesDash;

            for (var i = 0; i < _maxQueueSize; i++)
                EnqueueToQueue(() => DashOperationCount(1));

            _valueModelDash.SetValue(_maxQueueSize);

            ThreadPool.QueueUserWorkItem(async (state) => { await ProcessQueue(); });
        }

        public async UniTask AwaitLoadPlayerConfig(PlayerConfigs configs)
        {
            await UniTask.WaitUntil(() => configs.IsLoadAllConfig);
        }

        public async UniTask AwaitLoadWeaponConfigs(WeaponConfigs configs)
        {
            await UniTask.WaitUntil(() => configs.IsLoadConfigs);
        }

        public async UniTask AwaitLoadShakeCameraConfigs(CameraShakeConfigs cameraShakeConfigs)
        {
            await UniTask.WaitUntil(() => cameraShakeConfigs.IsLoadShakeConfigs);
        }

        public async UniTask AwaitLoadPrefabConfigs(WeaponPrefabs weaponPrefabs)
        {
            await UniTask.WaitUntil(() => weaponPrefabs.IsLoadConfigs);
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
            _dataWorkerStateMachine.StateMachineData.DashCount += sign;
            _valueModelDash.ChangeValue(_dataWorkerStateMachine.StateMachineData.DashCount);
            _dataWorkerStateMachine.StateMachineData.DashCount =
                Mathf.Clamp(_dataWorkerStateMachine.StateMachineData.DashCount, 0,
                    _dataWorkerStateMachine.PlayerConfigs.DashConfig.NumberChargesDash);
        }

        [Inject]
        private void Construct(IStateDataWorker dataWorker, WeaponData weaponData,
            ValueCountStorage<int> valueModelDash)
        {
            _dataWorkerStateMachine = dataWorker;
            _valueModelDash = valueModelDash;
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
                await UniTask.Delay(TimeSpan.FromSeconds(_dataWorkerStateMachine.PlayerConfigs.DashConfig.DashDelay));
                action();
            }
        }

    }
}