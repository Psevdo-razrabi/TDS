using System;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player.AnyScripts
{
    public class Landing : IInitializable, IDisposable
    {
        private StateMachineData _stateMachineData;
        private Player _player;
        private readonly LandingColliders _landingColliders;
        private CompositeDisposable _compositeDisposable = new();

        public Landing(Player player, LandingColliders landingColliders)
        {
            _player = player;
            _landingColliders = landingColliders;
            _stateMachineData = player.StateMachineData;
        }


        public void Initialize()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    CheckGround();
                })
                .AddTo(_compositeDisposable);
        }
        
        private void CheckGround()
        {
            if(Physics.Raycast(_player.transform.position, Vector3.down, 0.5f, LayerMask.GetMask("Ground")))
            {
                _stateMachineData.IsPlayerInObstacle = false;
            }
            
            if(Physics.Raycast(_player.transform.position, Vector3.down, 0.5f, LayerMask.GetMask("ClambingObject")))
            {
                _landingColliders.ActiveColliders(true);
                
                if (Physics.Raycast(_player.transform.position,Vector3.left, out RaycastHit hit, 0.7f, LayerMask.GetMask("LandingObject")))
                {
                    _stateMachineData.Rotation = Quaternion.LookRotation(-hit.normal);
                    _stateMachineData.IsGrounded.Value = false;
                }
                else if(Physics.Raycast(_player.transform.position, Vector3.right, out RaycastHit hit1, 0.7f, LayerMask.GetMask("LandingObject")))
                {
                    _stateMachineData.Rotation = Quaternion.LookRotation(-hit1.normal);
                    _stateMachineData.IsGrounded.Value = false;
                }
                else if (Physics.Raycast(_player.transform.position, Vector3.forward, out RaycastHit hit2, 0.7f, LayerMask.GetMask("LandingObject")))
                {
                    _stateMachineData.Rotation = Quaternion.LookRotation(-hit2.normal);
                    _stateMachineData.IsGrounded.Value = false;
                }
                else if(Physics.Raycast(_player.transform.position, Vector3.back, out RaycastHit hit3, 0.7f, LayerMask.GetMask("LandingObject")))
                {
                    _stateMachineData.Rotation = Quaternion.LookRotation(-hit3.normal);
                    _stateMachineData.IsGrounded.Value = false;
                }
                else
                {
                    _stateMachineData.IsGrounded.Value = true;
                }
            }
            else
            {
                _stateMachineData.IsGrounded.Value = true;
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable?.Clear();
        }
    }
}