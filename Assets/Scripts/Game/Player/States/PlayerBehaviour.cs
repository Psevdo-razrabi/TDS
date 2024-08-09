using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Interfase;
using PhysicsWorld;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class PlayerBehaviour : IState
    {
        protected readonly InitializationStateMachine StateMachine;
        protected CompositeDisposable Disposable = new();
        protected readonly Player Player;
        protected readonly StateMachineData Data;

        public virtual void OnEnter() => AddActionsCallbacks();

        public virtual void OnExit() => RemoveActionCallbacks();

        public virtual void OnUpdateBehaviour()
        {
            UpdateAnimatorInput();
            AimIsFreeze(Player.transform.rotation);
        }
        public virtual void OnFixedUpdateBehaviour() {}

        protected PlayerBehaviour(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData)
        {
            StateMachine = stateMachine;
            Player = player;
            Data = stateMachineData;
        }

        protected virtual void AddActionsCallbacks() {}

        protected virtual void RemoveActionCallbacks() {}

        protected virtual void GravityForce()
        {
            Player.CharacterController.Move(new Vector3(0f, Data.TargetDirectionY, 0f));
        }

        private void UpdateAnimatorInput()
        {
            Data.XInput = UnityEngine.Input.GetAxis("Horizontal");
            Data.YInput = UnityEngine.Input.GetAxis("Vertical");
        }
        
        protected async UniTask RotatePlayerToObstacle()
        {
            await Player.transform.DORotateQuaternion(Data.Rotation, 1f);
        }
        
        protected void AimIsFreeze(Quaternion rotation)
        {
            if (Data.IsLockAim)
            {
                Player.PlayerAim.FreezeAim(rotation);
            }
            else
            {
                Player.PlayerAim.Aim();
            }
        }
    }
}