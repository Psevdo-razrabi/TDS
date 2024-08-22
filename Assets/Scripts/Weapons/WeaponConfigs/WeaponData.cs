using System;
using System.Collections.Generic;
using Game.Player.Weapons.InterfaceWeapon;
using UniRx;
using UnityEngine;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class WeaponData : IDisposable
    {
        public Dictionary<BodyType, float> DamageForType { get; set; } = new();
        public bool IsReloading { get; set; }
        public bool IsShoot { get; private set; } = true;
        public ReactiveProperty<int> AmmoInMagazine { get; set; } 
        public Transform BulletPoint { get; set; }
        public FireStrategy FireStrategy { get; set; }

        private CompositeDisposable _compositeDisposable;

        public void Subscribe(Action soundDelegate)
        {
            _compositeDisposable = new CompositeDisposable();
            AmmoInMagazine
                .Subscribe(_ => IsShoot = AmmoInMagazine.Value > 0)
                .AddTo(_compositeDisposable);
            
            AmmoInMagazine
                .Where(_ => AmmoInMagazine.Value <= 1)
                .Subscribe(_ => soundDelegate())
                .AddTo(_compositeDisposable);
        }
        

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable?.Clear();
        }
    }
}