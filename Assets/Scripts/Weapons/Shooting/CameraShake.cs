using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraShake
{ 
    private CameraShakeConfigs _cameraShakeConfigs;
    private CameraShakeConfig _shakeConfig;
    private ICameraProvider _cameraProvider;
    
    private Vector3 _originalPosition;
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
        if (!_isShaking) {
            _originalPosition = _cameraProvider.CameraTransform.position;
        }
        
        _cameraProvider.CameraTransform.DOKill(complete: true);

        _isShaking = true;
        _cameraProvider.CameraTransform
            .DOShakePosition(_shakeConfig.ShakeDuration, _shakeConfig.ShakeStrength, 1, 90f, false, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InOutBounce)
            .SetLink(_cameraProvider.CameraTransform.gameObject)
            .OnComplete(() => {
                _cameraProvider.CameraTransform.position = _originalPosition;
                _isShaking = false;
            });
    }
   
}
