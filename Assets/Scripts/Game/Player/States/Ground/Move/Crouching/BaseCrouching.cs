using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Orientation;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class BaseCrouching : PlayerOrientation
    {
        public BaseCrouching(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        { }
        
        protected async UniTask InterpolatedFloatWithEase(float startValue, Action<float> setter, float endValue, float duration, AnimationCurve curve)
        {
            await DOTween
                .To(() => startValue, x => setter(x), endValue, duration)
                .SetEase(curve);
        }
        
        protected async UniTask InterpolatedVector3WithEase(Vector3 startValue, Action<Vector3> setter, Vector3 endValue, float duration, AnimationCurve curve)
        {
            await DOTween
                .To(() => startValue, x => setter(x), endValue, duration)
                .SetEase(curve);
        }
    }
}