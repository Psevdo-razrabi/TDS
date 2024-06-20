using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Customs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FOW;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomHideInFOG : HiderBehavior
{
    public Dictionary<string, List<Material>> Material { get; private set; }
    public readonly string KEY_FIRST = "EnemyShaderHideInFOG";
    public readonly string KEY_SECOND = "EnemyShaderStandart";
    [SerializeField] private GameObject canvas;
    [SerializeField] [Range(0, 15f)] private float timeToHideMaterial;
    private static readonly int ColorID = Shader.PropertyToID("_BaseColor");
    private SkinnedMeshRenderer[] _skinnedMeshRenderer;
    private CancellationTokenSource _cancellationTokenSource;

    public Action OnRevealFirstRealization { get; private set; }
    public Action OnHideFirstRealization { get; private set; }

    public readonly ReactiveProperty<bool> IsHideEnemyInFog = new();
 
    public override async void OnReveal()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        IsHideEnemyInFog.Value = false;
        await UniTask.WhenAll(Material[KEY_SECOND].Select(material => InterpolateAlpha(1f, material)));
        OperationWithHide(ShadowCastingMode.On, true, false);
    }

    public override async void OnHide()
    {
        _cancellationTokenSource?.Cancel();
        IsHideEnemyInFog.Value = true;
        OperationWithHide(ShadowCastingMode.Off, false, true);
        _cancellationTokenSource = new CancellationTokenSource();
        await UniTask.WhenAll(Material[KEY_SECOND].Select(material => InterpolateAlpha(0f, material)));
    }

    private void OperationWithHide(ShadowCastingMode shadowCastingMode, bool isActiveCanvas, bool isMaterialTransparent)
    {
        _skinnedMeshRenderer.ForEach(mesh => mesh.shadowCastingMode = shadowCastingMode);
        canvas.SetActive(isActiveCanvas);
        Material[KEY_SECOND].ForEach(material => material.SetMaterialTransparent(isMaterialTransparent));
    }
    
    private void Awake()
    {
        InitDictionary();
        _skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();

        var matEnemyFirst = new List<Material>();
        var matEnemySecond = new List<Material>();
        
        _skinnedMeshRenderer.ForEach(mesh =>
        {
            var materials = mesh.materials;
            matEnemyFirst.Add(materials.FirstOrDefault(mat => mat.name == "Enemy (Instance)"));
            matEnemySecond.Add(materials.FirstOrDefault(mat => mat.name != "Enemy (Instance)"));
        });

        Material[KEY_FIRST] = matEnemyFirst;
        Material[KEY_SECOND] = matEnemySecond;
        
        OnHide();
    }

    private void OnDestroy()
    {
        OnRevealFirstRealization = OnHideFirstRealization = null;
    }

    private void InitDictionary()
    {
        Material = new Dictionary<string, List<Material>>
        {
            { KEY_FIRST, null },
            { KEY_SECOND, null }
        };
    }
    
    private async UniTask InterpolateAlpha(float endValue, Material material)
    {
        await DOTween
            .To(() => material.GetVector(ColorID).w, x =>
                {
                    var vectorColor = material.GetColor(ColorID);
                    vectorColor.a = x;
                    material.SetColor(ColorID, vectorColor);
                },
                endValue, timeToHideMaterial)
            .WithCancellation(_cancellationTokenSource.Token);
    }
}
