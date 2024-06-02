using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.VFX;

public class DashTrailEffect : MonoBehaviour
{
    [SerializeField] private float activeTime = 2f;
    [Header("Mesh Related")]
    [SerializeField] private float meshRefreshRate = 0.1f;
    [SerializeField] private float meshDestroyDelay = 3f;
    [SerializeField] private Transform positionToSpawn;
    [Header("Shader Related")]
    [SerializeField] private Material materialShader;
    [SerializeField] private float shaderVarRate = 0.1f;
    [SerializeField] private float shaderVarRefreshRate = 0.05f;
    [Header("VFX Related")]
    [SerializeField] private VisualEffect vfxGraphParticlesTrail;
    [SerializeField] private VisualEffect vfxGraphInitialImpact;
    [SerializeField] private float timeToStopVFXEffect;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    private bool _isTrailActive;
    private const string ShaderVarRef = "_Alpha";
    private static readonly int Alpha = Shader.PropertyToID(ShaderVarRef);

    private void Start()
    {
        VFXStop();
    }
    
    public async void ActivateVFXEffectDash()
    {
        if (_isTrailActive) return;
        
        _isTrailActive = true;
        await ActivateTrail();
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
                _isTrailActive = false;
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
        var gameObjVFX = new GameObject();
        gameObjVFX.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
        var meshRendererVFX = gameObjVFX.AddComponent<MeshRenderer>();
        var meshFilter = gameObjVFX.AddComponent<MeshFilter>();
        var mesh = new Mesh();
        meshRenderer.BakeMesh(mesh);
        meshFilter.mesh = mesh;
        meshRendererVFX.material = materialShader;
        await AnimateMaterialFloat(meshRendererVFX.material, 0, shaderVarRate, shaderVarRefreshRate);
        Destroy(gameObjVFX, meshDestroyDelay);
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
