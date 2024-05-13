using System;
using Zenject;

namespace UI.Storage
{
    public class BoolStorage
    {
        public event Action<bool> OnBoolValueChange;
        
        public void ChangeBoolValue(bool value)
        {
            OnBoolValueChange?.Invoke(value);
        }
    }
}