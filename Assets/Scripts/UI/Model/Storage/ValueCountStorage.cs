using System;
using UnityEngine;

namespace UI.Storage
{
    public sealed class ValueCountStorage<T>
    {
        public event Action<T> OnCountValueChange;
        private T _value;

        public T Value
        {
            get => _value;
            private set
            {
                if (!typeof(T).IsValueType)
                {
                    throw new ArgumentException("ValueCountStorage can only be used with value types.");
                }

                _value = value;
                OnCountValueChange?.Invoke(Value);
            }
        }

        public void SetValue(T value) => Value = value;

        public void ChangeValue(T value)
        {
            Value = value;
        }

        public T GetValue() => Value;
    }
}