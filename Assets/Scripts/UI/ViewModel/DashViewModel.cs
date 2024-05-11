using System;
using MVVM;
using UI.Storage;
using UniRx;
using IInitializable = Zenject.IInitializable;

namespace UI.ViewModel
{
    public class DashViewModel : IInitializable, IDisposable
    {
        [Data("TextInScreen")] 
        public readonly ReactiveProperty<string> Dash = new();
        private ValueCountStorage<int> _valueCountStorage;

        public DashViewModel(ValueCountStorage<int> valueCountStorage)
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
            Dash.Value = $"Dash Count: {count}";
        }
    }
}