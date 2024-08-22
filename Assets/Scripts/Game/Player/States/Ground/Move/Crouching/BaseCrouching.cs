using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.AnyScripts;
using Game.Player.States.Orientation;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class BaseCrouching : PlayerOrientation
    {
        protected CancellationTokenSource Cancellation = new();
        public BaseCrouching(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        { }
        
        protected async UniTask InterpolatedFloatWithEase(float startValue, Action<float> setter, float endValue, float duration, AnimationCurve curve, CancellationToken token)
        {
            await DOTween
                .To(() => startValue, x => setter(x), endValue, duration)
                .SetEase(curve)
                .WithCancellation(cancellationToken: token);
        }
        
        protected async UniTask InterpolatedVector3WithEase(Vector3 startValue, Action<Vector3> setter, Vector3 endValue, float duration, AnimationCurve curve, CancellationToken token)
        {
            await DOTween
                .To(() => startValue, x => setter(x), endValue, duration)
                .SetEase(curve)
                .WithCancellation(cancellationToken: token);
        }

        protected void CreateTokenAndDelete()
        {
            Cancellation?.Cancel();
            Cancellation = new CancellationTokenSource();
        }
    }
}