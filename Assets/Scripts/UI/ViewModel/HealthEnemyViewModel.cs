using System;
using MVVM;
using UI.Storage;
using UniRx;
using Zenject;

namespace UI.ViewModel
{
    public class HealthEnemyViewModel : IDisposable, IInitializable
    {
        [Data("Image")] 
        public readonly ReactiveProperty<float> HealthFillImage = new();
        
        private ValueCountStorage<float> _valueCountStorage;

        public HealthEnemyViewModel(ValueCountStorage<float> valueCountStorage)
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