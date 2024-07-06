using System;
using System.Collections.Generic;
using FOW;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Zenject;

public class HiderManager : ITickable, IInitializable, IDisposable
{
    private FogOfWarRevealer3D _revealer;
    private Dictionary<FogOfWarHider, CustomHideInFOG> _dictionary = new();
    private CompositeDisposable _compositeDisposable = new();
    public HiderManager(FogOfWarRevealer3D revealer)
    {
        _revealer = revealer;
    }

    private void Subscribe()
    {
        _revealer.hidersSeen
            .ObserveAdd()
            .Subscribe(fog =>
        {
            FindKeyInDictionary(key =>
            {
                if(fog.Value != key) return;
                _dictionary[key].OnReveal();
            });
        }).AddTo(_compositeDisposable);

        _revealer.hidersSeen
            .ObserveRemove()
            .Subscribe(fog =>
        {
            FindKeyInDictionary(key =>
            {
                if(fog.Value != key) return;
                _dictionary[key].OnHide();
            });
        }).AddTo(_compositeDisposable);
    }

    private void FindKeyInDictionary(Action<FogOfWarHider> eventInCollection)
    {
        foreach (var key in _dictionary.Keys)
        {
            eventInCollection.Invoke(key);
        }
    }

    private void AddInDictionary()
    {
        if (_revealer.hidersSeen.Count == 0) return;

        _revealer.hidersSeen.ForEach(hider =>
        {
            if (!_dictionary.ContainsKey(hider))
            {
                _dictionary.Add(hider, hider.gameObject.GetComponent<CustomHideInFOG>());
            }
        });
    }

    public void Tick()
    {
        AddInDictionary();
    }
    
    public void Initialize()
    {
        Subscribe();
    }

    public void Dispose()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable?.Clear();
    }
}
