using System;
using Game.AsyncWorker;
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
        protected AsyncWorker AsyncWorker;
        
        
        [Inject]
        private void Construct(InputSystem input, PlayerConfigs playerConfigs, AsyncWorker worker)
        {
            InputSystemNew = input ?? throw new ArgumentNullException($"{nameof(input)} is null");
            InputSystemNew.Enable();
            PlayerConfigs = playerConfigs;
            AsyncWorker = worker;
        }
    }
}