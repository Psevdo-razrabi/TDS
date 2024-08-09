using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class RotateModel : MonoBehaviour
    {
        [field: SerializeField] private GameObject objRotation;
        private StateMachineData _stateMachineData;

        [Inject]
        private void Construct(StateMachineData stateMachineData) => _stateMachineData = stateMachineData;
        
        private void Awake()
        {
            Observable
                .EveryUpdate()
                .Where(_ => _stateMachineData.IsLockAim == false)
                .Subscribe(_ => transform.rotation = objRotation.transform.rotation)
                .AddTo(this);
        }
    }
}