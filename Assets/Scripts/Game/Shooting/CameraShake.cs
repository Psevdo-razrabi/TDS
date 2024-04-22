using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class CameraShake
{
   private Transform _cameraTransform;
   
   [Inject]
   public void Construct(Transform camera)
   {
      _cameraTransform = camera;
   }

   public void ShakeCamera(CameraShakeConfig cameraShakeConfig)
   {
      _cameraTransform.DOShakePosition(cameraShakeConfig.ShakeDuration, cameraShakeConfig.ShakeStrength, 1, 90f, false, true, ShakeRandomnessMode.Harmonic)
         .SetEase(Ease.InOutBounce).SetLink(_cameraTransform.gameObject);
   }
}
