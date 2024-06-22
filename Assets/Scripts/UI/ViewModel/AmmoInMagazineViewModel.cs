using System;
using MVVM;
using UI.Storage;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModel
{
    public class AmmoInMagazineViewModel : IInitializable, IDisposable
    {
        [Data("AmmoInMagazine")]
        public readonly ReactiveProperty<string> Ammo = new();

        private ValueCountStorage<int> _valueCountStorage;

        public AmmoInMagazineViewModel(ValueCountStorage<int> valueCountStorage)
        {
            _valueCountStorage = valueCountStorage;
        }

        public void Initialize()
        {
            OnValueCountChange(_valueCountStorage.Value);
            _valueCountStorage.OnCountValueChange += OnValueCountChange;
        }

        public void Dispose()
        {
            _valueCountStorage.OnCountValueChange -= OnValueCountChange;
        }
        
        private void OnValueCountChange(int count)
        {
            Ammo.Value = count.ToString();
        }
    }
}