using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DetectingEnemies : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float durationAlpha;
    private CustomHideInFOG _customHideInFog;
    private static readonly int DitherID = Shader.PropertyToID("_DitherParameters");
    private List<Material> _enemyDetecting;

    private readonly CompositeDisposable _compositeDisposable = new();

    private void Start()
    {
        SubscribeTrigger(GetComponent<Collider>());
    }

    private void SubscribeTrigger(Collider collider)
    {
        collider
            .OnTriggerEnterAsObservable()
            .Do(CheckComponent)
            .Where(_ => _customHideInFog.IsHideEnemyInFog.Value)
            .Subscribe(async col=>
            {
                CheckComponent(col);
                await UniTask.WhenAll(_enemyDetecting.Select(material => InterpolatePropertyShader(1f, material)));

            }).AddTo(_compositeDisposable);
        
        collider
            .OnTriggerExitAsObservable()
            .Subscribe(async col=>
            {
                CheckComponent(col);
                await UniTask.WhenAll(_enemyDetecting.Select(material => InterpolatePropertyShader(0f, material)));
            }).AddTo(_compositeDisposable);
    }
    
    private async UniTask InterpolatePropertyShader(float endValue, Material material)
    {
        await DOTween
            .To(() => material.GetFloat(DitherID), x =>
                {
                    material.SetFloat(DitherID, x);
                },
                endValue, durationAlpha);
    }

    private void CheckComponent(Collider collider)
    {
        if (!collider.TryGetComponent(out Enemy.Enemy enemy) & !collider.TryGetComponent(out CustomHideInFOG customHide)) return;
        _customHideInFog = customHide;
        _enemyDetecting = _customHideInFog.Material[_customHideInFog.KEY_FIRST];
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
        _compositeDisposable.Clear();
    }
}
