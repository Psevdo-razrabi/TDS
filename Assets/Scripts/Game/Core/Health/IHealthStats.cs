﻿using Cysharp.Threading.Tasks;

namespace Game.Core.Health
{
    public interface IHealthStats
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }
        void SetDamage(float value);
        UniTaskVoid AddHealth(float value);
    }
}