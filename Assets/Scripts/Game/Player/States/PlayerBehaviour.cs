using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Interfase;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class PlayerBehaviour : IState
    {
        protected CompositeDisposable Disposable = new();
        protected readonly Player Player;
        protected readonly StateMachineData Data;

        public virtual void OnEnter() => AddActionsCallbacks();

        public virtual void OnExit() => RemoveActionCallbacks();

        public virtual void OnUpdateBehaviour()
        {
            UpdateAnimatorInput();
            AimIsFreeze(Player.PlayerComponents.transform.rotation);
        }
        public virtual void OnFixedUpdateBehaviour() {}

        protected PlayerBehaviour(PlayerStateMachine stateMachine)
        {
            Player = stateMachine.Player;
            Data = stateMachine.Data;
        }

        protected virtual void AddActionsCallbacks() {}

        protected virtual void RemoveActionCallbacks() {}

        protected virtual void GravityForce()
        {
            Player.PlayerComponents.CharacterController.Move(new Vector3(0f, Data.TargetDirectionY, 0f));
        }

        private void UpdateAnimatorInput()
        {
            Data.XInput = UnityEngine.Input.GetAxis("Horizontal");
            Data.YInput = UnityEngine.Input.GetAxis("Vertical");
        }
        
        protected async UniTask RotatePlayerToObstacle()
        { 
            var rotateModel =  Player.PlayerView.ModelRotate.transform.DORotateQuaternion(Data.Rotation, 1f);
            var rotatePlayer = Player.PlayerComponents.transform.DORotateQuaternion(Data.Rotation, 1f);

            await UniTask.WhenAll(rotatePlayer.ToUniTask(), rotateModel.ToUniTask());
        }

        protected void ZeroingRotation()
        {
            Player.PlayerComponents.transform.rotation = Quaternion.identity;
        }
        
        protected void AimIsFreeze(Quaternion rotation)
        {
            if (Data.IsLockAim)
            {
                Player.PlayerComponents.PlayerAim.FreezeAim(rotation, Player);
            }
            else
            {
                Player.PlayerComponents.PlayerAim.Aim();
            }
        }
    }
}