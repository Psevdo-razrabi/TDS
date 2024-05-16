using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraShake
{ 
    private CameraShakeConfigs _cameraShakeConfigs;
    private CameraShakeConfig _shakeConfig;
    private ICameraProvider _cameraProvider;

    private Vector3 _shakeOffset = Vector3.zero;
    private bool _isShaking = false;

    public CameraShake(CameraShakeConfigs shakeConfigs, ICameraProvider cameraProvider)
    {
        _cameraShakeConfigs = shakeConfigs;
        _shakeConfig = _cameraShakeConfigs.RifleShakeConfig;
        _cameraProvider = cameraProvider;
        LoadConfigs();
    }

    private async void LoadConfigs()
    {
        while (_cameraShakeConfigs.IsLoadShakeConfigs == false)
            await UniTask.Yield();

        _shakeConfig = _cameraShakeConfigs.RifleShakeConfig;
    }

    public void ShakeCamera()
    {
        if (_isShaking) return;
        
        _isShaking = true;

        _cameraProvider.CameraTransform.DOKill(complete: true);

        _cameraProvider.CameraTransform
            .DOShakePosition(_shakeConfig.ShakeDuration, _shakeConfig.ShakeStrength, 5, 90f, false, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InOutBounce)
            .OnUpdate(() =>
            {
                Vector3 currentNoShakePosition = _cameraProvider.CameraTransform.position - _shakeOffset;
                _shakeOffset = _cameraProvider.CameraTransform.position - currentNoShakePosition;
                _cameraProvider.CameraTransform.position = currentNoShakePosition;
            })
            .OnComplete(() =>
            {
                _shakeOffset = Vector3.zero;
                _isShaking = false;
            });
    }
}
