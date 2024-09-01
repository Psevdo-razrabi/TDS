using System;
using CharacterOrEnemyEffect.Interfaces;
using Game.AsyncWorker.Interfaces;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player.States.Buffer
{
    public class BufferAction : IInitializable, IDisposable
    {
        private readonly IIsTrailActive _dashEffect;
        private readonly IAwaiter _awaiter;
        public Action lastStateAction;
        public readonly ReactiveProperty<bool> IsBufferAlready = new();
        private readonly CompositeDisposable _compositeDisposable = new();
        private StateMachineData StateMachineData { get; }

        public BufferAction(StateMachineData stateMachineData, IIsTrailActive dashEffect, IAwaiter awaiter)
        {
            _dashEffect = dashEffect;
            _awaiter = awaiter;
            StateMachineData = stateMachineData;
        }

        public async void Initialize()
        {
            await _awaiter.AwaitLoadOrInitializeParameter(StateMachineData);
            IsBufferAlready
                .Subscribe(_ => InvokeActionBuffer())
                .AddTo(_compositeDisposable);

            StateMachineData.GetValue<ReactiveProperty<bool>>("IsDashing")
                .Subscribe(_ => InvokeActionBuffer())
                .AddTo(_compositeDisposable);
            
            _dashEffect.IsTrailActive
                .Subscribe(_ => InvokeActionBuffer())
                .AddTo(_compositeDisposable);
        }

        private void InvokeActionBuffer()
        {
            MainThreadDispatcher.Post(_ =>
            {
                if (!IsBufferAlready.Value || StateMachineData.GetValue<ReactiveProperty<bool>>(Name.IsDashing).Value 
                                           || _dashEffect.IsTrailActive.Value) return;
                InvokeState();
                IsBufferAlready.Value = false;
                Debug.LogWarning("сколько раз я вызвался?");
            }, null);
        }


        public void Dispose()
        {
            _compositeDisposable.Dispose();
            _compositeDisposable.Clear();
        }
        
        private void InvokeState() => lastStateAction?.Invoke();
    }
}