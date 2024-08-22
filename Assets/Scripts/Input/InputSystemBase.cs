using System;
using Game.AsyncWorker.Interfaces;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using UniRx;
using Zenject;

namespace Input
{
    public class InputSystemBase : IInitializable, IDisposable
    {
        protected readonly CompositeDisposable CompositeDisposable = new();
        protected InputSystem InputSystemNew;
        protected PlayerConfigs PlayerConfigs;
        protected IAwaiter AsyncWorker;
        protected InputObserver InputObserver;
        protected StateMachineData Data;
        protected PlayerComponents PlayerComponents;

        public InputSystemBase(PlayerComponents playerComponents, StateMachineData data, InputObserver inputObserver, IAwaiter asyncWorker, PlayerConfigs playerConfigs, InputSystem inputSystemNew)
        {
            inputSystemNew.Enable();
            PlayerComponents = playerComponents;
            Data = data;
            InputObserver = inputObserver;
            AsyncWorker = asyncWorker;
            PlayerConfigs = playerConfigs;
            InputSystemNew = inputSystemNew;
        }

        public void Initialize()
        {
            AddActionsCallbacks();
        }

        public void Dispose()
        {
            RemoveActionCallbacks();
        }
        
        protected virtual void AddActionsCallbacks() {}
        protected virtual void RemoveActionCallbacks() {}
    }
}