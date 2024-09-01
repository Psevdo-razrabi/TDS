using System;
using Cysharp.Threading.Tasks;
using Game.AsyncWorker.Interfaces;
using Game.Player.PlayerStateMashine;
using Input.Interface;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player.AnyScripts
{
    public class Landing : IInitializable, IDisposable
    {
        private readonly StateMachineData _stateMachineData;
        private readonly PlayerComponents _playerComponents;
        private readonly IMove _move;
        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly IAwaiter _awaiter;
        private Vector3 _direction;

        public Landing(IMove move, StateMachineData data, PlayerComponents playerComponents, IAwaiter awaiter)
        {
            _move = move;
            _stateMachineData = data;
            _playerComponents = playerComponents;
            _awaiter = awaiter;
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
        
        private async void Subscribes()
        {
            await _awaiter.AwaitLoadOrInitializeParameter(_stateMachineData.PlayerConfigs);
            Observable
                .EveryUpdate()
                .Subscribe(_ => CheckLandingOrGround())
                .AddTo(_compositeDisposable);

            _move.MoveNonInterpolated
                .Subscribe(vector2 => _direction = new Vector3(vector2.x, 0f, vector2.y))
                .AddTo(_compositeDisposable);
        }
        
        private void CheckLandingOrGround()
        {
            var playerLandingConfig = _stateMachineData.PlayerConfigs.ObstacleConfigsProvider.LandingConfig;
            var checkClampingObject = CheckPlane(playerLandingConfig.ClambingObject.LayerMask, Vector3.down, playerLandingConfig.ClambingObject.CastDistance);
            if (checkClampingObject.isGround)
            {
                var checkLandingObject =
                    CheckPlane(playerLandingConfig.LandingObject.LayerMask, _direction, playerLandingConfig.LandingObject.CastDistance);
                _stateMachineData.SetValue(Name.Rotation, Quaternion.LookRotation(-checkLandingObject.hit.normal));
                _stateMachineData.GetValue<ReactiveProperty<bool>>(Name.IsGrounded).Value = !checkLandingObject.isGround;
            }
            else
            {
                _stateMachineData.GetValue<ReactiveProperty<bool>>(Name.IsGrounded).Value = CheckPlane(playerLandingConfig.Ground.LayerMask, Vector3.down, playerLandingConfig.Ground.CastDistance).isGround;
            }
        }

        private (bool isGround, RaycastHit hit) CheckPlane(LayerMask layerMask, Vector3 direction, float castDistance)
        {
            return (Physics.Raycast(_playerComponents.transform.position, direction, out RaycastHit hit,
                castDistance, layerMask), hit);
        }
    }
}