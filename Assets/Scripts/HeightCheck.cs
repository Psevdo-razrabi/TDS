using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class HeightCheck : MonoBehaviour
{
    [SerializeField] private List<LayerMask> _floorLayer;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _spherePoint;
    [SerializeField] private float topSurface;
    
    private CompositeDisposable _compositeDisposable = new();
    private CrosshairRaycast _crosshairRaycast;
    private bool smth = true;
    
    [Inject]
    private void Construct(CrosshairRaycast crosshairRaycast)
    {
        _crosshairRaycast = crosshairRaycast;
        
        Observable
            .EveryUpdate()
            .Where(_ => smth == true)
            .Subscribe(_ => CheckHeight())
            .AddTo(_compositeDisposable);
    }
    
    public Vector3? CheckHeight()
    {
        LayerMask combinedLayerMask = PlusLayer();
        Ray ray = _crosshairRaycast.RayCast;
        
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedLayerMask))
        {
            Vector3 upVector = Vector3.up;
            float dotProduct = Vector3.Dot(hit.normal, upVector);

            if (dotProduct >= topSurface)
            {
                _spherePoint.position = hit.point;
                return hit.point;
            }
        }
        return null;
    }
    
    private LayerMask PlusLayer()   
    {
        int plusedLayer = 0;

        foreach (LayerMask layerMask in _floorLayer)
        {
            plusedLayer |= layerMask.value;
        }
        return plusedLayer;
    }
}
