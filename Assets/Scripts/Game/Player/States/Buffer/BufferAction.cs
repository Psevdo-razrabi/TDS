using System;
using CharacterOrEnemyEffect.Interfaces;
using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEditor.Search;
using UnityEngine;
using Zenject;

namespace Game.Player.States.Buffer
{
    public class BufferAction : IInitializable, IDisposable
    {
        private readonly IIsTrailActive _dashEffect;
        public Action lastStateAction;
        public readonly ReactiveProperty<bool> IsBufferAlready = new();
        private readonly CompositeDisposable _compositeDisposable = new();
        private StateMachineData StateMachineData { get; }

        public BufferAction(StateMachineData stateMachineData, IIsTrailActive dashEffect)
        {
            _dashEffect = dashEffect;
            StateMachineData = stateMachineData;
        }

        public void Initialize()
        {
            IsBufferAlready
                .Subscribe(_ => InvokeActionBuffer())
                .AddTo(_compositeDisposable);

            StateMachineData.IsDashing
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
                if (!IsBufferAlready.Value || StateMachineData.IsDashing.Value 
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