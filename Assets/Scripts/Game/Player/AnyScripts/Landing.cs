using System;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Input.Interface;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player.AnyScripts
{
    public class Landing : IInitializable, IDisposable
    {
        private StateMachineData _stateMachineData;
        private Player _player;
        private readonly PlayerInLandingConfig _playerInLandingConfig;
        private readonly IMove _move;
        private CompositeDisposable _compositeDisposable = new();
        private Vector3 _direction;

        public Landing(Player player, PlayerInLandingConfig playerInLandingConfig, IMove move)
        {
            _player = player;
            _playerInLandingConfig = playerInLandingConfig;
            _move = move;
            _stateMachineData = player.StateMachineData;
        }
        
        public void Initialize()
        {
            Subscribes();
        }
        
        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable?.Clear();
        }
        
        private void Subscribes()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => CheckGround())
                .AddTo(_compositeDisposable);

            _move.MoveNonInterpolated
                .Subscribe(vector2 => _direction = new Vector3(vector2.x, 0f, vector2.y))
                .AddTo(_compositeDisposable);
        }
        
        private void CheckGround()
        {
            var checkClampingObject = CheckPlane(_playerInLandingConfig.ClambingObject.LayerMask, Vector3.down, _playerInLandingConfig.ClambingObject.CastDistance);
            if (checkClampingObject.isGround)
            {
                var checkLandingObject =
                    CheckPlane(_playerInLandingConfig.LandingObject.LayerMask, _direction, _playerInLandingConfig.LandingObject.CastDistance);
                _stateMachineData.Rotation = Quaternion.LookRotation(-checkLandingObject.hit.normal);
                _stateMachineData.IsGrounded.Value = !checkLandingObject.isGround;
            }
            else
            {
                _stateMachineData.IsGrounded.Value = CheckPlane(_playerInLandingConfig.Ground.LayerMask, Vector3.down, _playerInLandingConfig.Ground.CastDistance).isGround;
            }
            
        }

        private (bool isGround, RaycastHit hit) CheckPlane(LayerMask layerMask, Vector3 direction, float castDistance)
        {
            return (Physics.Raycast(_player.transform.position, direction, out RaycastHit hit,
                castDistance, layerMask), hit);
        }
    }
}