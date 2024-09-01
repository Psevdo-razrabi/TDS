using System;
using System.Collections;
using System.Collections.Generic;
using Game.AsyncWorker.Interfaces;
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
    private IAwaiter _awaiter;

    public IObservable<Vector3?> AimPointUpdates => _aimPointSubject.AsObservable();
    
    [Inject]
    private void Construct(StateMachineData stateMachineData, CrosshairRaycast crosshairRaycast, IAwaiter awaiter)
    {
        _stateMachineData = stateMachineData;
        _crosshairRaycast = crosshairRaycast;
        _awaiter = awaiter;
    }

    private async void Start()
    {
        await _awaiter.AwaitLoadOrInitializeParameter(_stateMachineData);
        SubscribeCalculateAim();
    }

    private void SubscribeCalculateAim()
    {
        var isAimingStream = _stateMachineData.GetValue<ReactiveProperty<bool>>(Name.IsAiming)
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
