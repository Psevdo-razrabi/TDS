using System;
using CharacterOrEnemyEffect;
using CharacterOrEnemyEffect.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

public class DashTrailEffect : MonoBehaviour, IIsTrailActive
{
    [SerializeField] private float activeTime = 2f;
    [Header("Mesh Related")]
    [SerializeField] private float meshRefreshRate = 0.1f;
    [SerializeField] private float meshDestroyDelay = 3f;
    [Header("Transforms")]
    [SerializeField] private Transform playerDirectionTransform;
    [SerializeField] private Transform playerRotationTransform;
    [Header("Shader Related")]
    [SerializeField] private Material materialShader;
    [SerializeField] private float shaderVarRate = 0.1f;
    [SerializeField] private float shaderVarRefreshRate = 0.05f;
    [Header("VFX Related")]
    [SerializeField] private VisualEffect vfxGraphParticlesTrail;
    [SerializeField] private VisualEffect vfxGraphInitialImpact;
    [SerializeField] private float timeToStopVFXEffect;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    private const string ShaderVarRef = "_Alpha";
    private static readonly int Alpha = Shader.PropertyToID(ShaderVarRef);
    private CreateVFXTrail _createVFXTrail;

    public ReactiveProperty<bool> IsTrailActive { get; private set; } = new();

    [Inject]
    private void Construct(CreateVFXTrail createVFXTrail) => _createVFXTrail = createVFXTrail;
    
    private void Start()
    {
        VFXStop();
    }
    
    public async void ActivateVFXEffectDash()
    {
        if (IsTrailActive.Value) return;
        
        IsTrailActive.Value = true;
        await ActivateTrail();
        IsTrailActive.Value = false;
    }
    
    private async UniTask ActivateTrail()
    {
        VFXStart();

        await DOTween
            .To(() => 0f, x => SpawnMeshVFX().Forget(), activeTime, meshRefreshRate)
            .SetEase(Ease.Linear)
            .OnComplete(async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(timeToStopVFXEffect));
                VFXStop();
            });
    }

    private async UniTaskVoid SpawnMeshVFX()
    {
        _skinnedMeshRenderers ??= GetComponentsInChildren<SkinnedMeshRenderer>();
        _skinnedMeshRenderers.ForEach(CreateObjectVFXTrail);
        await UniTask.Delay(TimeSpan.FromSeconds(meshRefreshRate));
    }

    private async void CreateObjectVFXTrail(SkinnedMeshRenderer meshRenderer)
    {
        var effect = _createVFXTrail.Create();
        effect.gameObject.transform.SetPositionAndRotation(playerDirectionTransform.position, playerRotationTransform.rotation);
        meshRenderer.BakeMesh(effect.mesh);
        effect.meshFilter.mesh = effect.mesh;
        effect.meshRenderer.material = materialShader;
        await AnimateMaterialFloat(effect.meshRenderer.material, 0, shaderVarRate, shaderVarRefreshRate);
        await UniTask.Delay(TimeSpan.FromSeconds(meshDestroyDelay));
        effect.gameObject.SetActive(false);
    }

    
    private async UniTask AnimateMaterialFloat(Material material, float goal, float rate, float refreshRate)
    {
        var valueToAnimate = material.GetFloat(Alpha);

        await DOTween.To(() => goal, async x =>
        {
            material.SetFloat(Alpha, valueToAnimate);
            await UniTask.Delay(TimeSpan.FromSeconds(refreshRate));
        }, valueToAnimate, rate).SetEase(Ease.Linear);
    }

    private void VFXStart()
    {
        vfxGraphParticlesTrail.Play();
        vfxGraphInitialImpact.Play();
    }

    private void VFXStop()
    {
        vfxGraphParticlesTrail.Stop();
        vfxGraphInitialImpact.Stop();
    }
}
