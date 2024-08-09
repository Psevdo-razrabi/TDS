using System;
using System.Collections;
using System.Collections.Generic;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;
using Zenject;

public class AimRay : MonoBehaviour
{
    private readonly CompositeDisposable _compositeDisposable = new();
    private readonly Subject<Vector3?> _aimPointSubject = new();
    
    private StateMachineData _stateMachineData;
    private CrosshairRaycast _crosshairRaycast;
    
    public IObservable<Vector3?> AimPointUpdates => _aimPointSubject.AsObservable();
    
    [Inject]
    private void Construct(StateMachineData stateMachineData, CrosshairRaycast crosshairRaycast)
    {
        _stateMachineData = stateMachineData;
        _crosshairRaycast = crosshairRaycast;
    }

    private void Awake()
    {
       SubscribeCalculateAim();
    }

    private void SubscribeCalculateAim()
    {
        var isAimingStream = _stateMachineData.IsAiming
            .DistinctUntilChanged();

        Observable
            .EveryUpdate()
            .WithLatestFrom(isAimingStream, (_, isAiming) => isAiming)
            .Subscribe(isAiming =>
            {
                if (isAiming == true)
                {
                    var hitPoint = CheckComponent();
                    _aimPointSubject.OnNext(hitPoint);
                }
                else
                {
                    _aimPointSubject.OnNext(null);
                }
            })
            .AddTo(_compositeDisposable);
    }
    public Vector3? CheckComponent()
    {
        Ray ray = _crosshairRaycast.RayCast;
        RaycastHit hit; 

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out BodyAim bodyType))
            {
                Debug.Log(bodyType.BodyPart);
                return hit.point;
            }
        }
        return null;
    }
}
