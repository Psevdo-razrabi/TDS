using DG.Tweening;
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

}
