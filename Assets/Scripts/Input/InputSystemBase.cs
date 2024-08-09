using System;
using Game.AsyncWorker;
using Game.AsyncWorker.Interfaces;
using Game.Player;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;
using Zenject;

namespace Input
{
    public class InputSystemBase : MonoBehaviour
    {
        protected InputSystem InputSystemNew;
        protected readonly CompositeDisposable CompositeDisposable = new();
        protected PlayerConfigs PlayerConfigs;
        protected IAwaiter AsyncWorker;
        protected InputObserver InputObserver;
        protected Player Player;
        protected StateMachineData Data;
        
        
        [Inject]
        private void Construct(InputSystem input, PlayerConfigs playerConfigs, IAwaiter worker, InputObserver inputObserver, Player player, StateMachineData stateMachineData)
        {
            InputSystemNew = input ?? throw new ArgumentNullException($"{nameof(input)} is null");
            InputSystemNew.Enable();
            PlayerConfigs = playerConfigs;
            AsyncWorker = worker;
            InputObserver = inputObserver;
            Player = player;
            Data = stateMachineData;
        }
    }
}