using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UniRx;
using UnityEngine;
using Zenject;

public class CrosshairRaycast : MonoBehaviour
{
    private Crosshair _crosshair;
    private CompositeDisposable _compositeDisposable = new();
    private Ray _ray;
    
    private bool _canRaycasting = true;

    public Ray RayCast => _ray;
    
    [Inject]
    private void Construct(Crosshair crosshair)
    {
        _crosshair = crosshair;
        
        Observable
            .EveryUpdate()
            .Where(_ => _canRaycasting == true)
            .Subscribe(_ => ReleaseRay())
            .AddTo(_compositeDisposable);
    }

    private void ReleaseRay()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, _crosshair.CrossHair.position);
        _ray = mainCamera.ScreenPointToRay(screenPoint);
        
         Debug.DrawRay(_ray.origin, _ray.direction * 10, Color.red, 1f);
    }
}
