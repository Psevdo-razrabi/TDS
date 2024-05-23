using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Storage
{
    public class AmmoStorage
    {
        public event Action<int> OnValueChanged;
    }
}