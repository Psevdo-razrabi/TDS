using System;
using MVVM;
using UI.Storage;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ViewModel
{
    public class HealthViewModel : IDisposable, IInitializable
    {
        [Data("Image")] 
        public readonly ReactiveProperty<float> HealthFillImage = new();
        [Data("GameObject")]
        public readonly ReactiveProperty<GameObject> GameObject = new();
        
        private ValueCountStorage<float> _valueCountStorage;

        public HealthViewModel(ValueCountStorage<float> valueCountStorage)
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
        
        private void OnValueCountChange(float count)
        {
            HealthFillImage.Value = count;
        }
    }
}