using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class WeaponData
    {
        public Dictionary<BodyType, float> DamageForType { get; set; } = new();
        public bool IsReloading { get; set; }
        public bool IsShoot { get; private set; } = true;
        public ReactiveProperty<int> AmmoInMagazine { get; set; } 
        public Transform BulletPoint { get; set; }

        private CompositeDisposable _compositeDisposable;

        public void SubscribeCanShot()
        {
            _compositeDisposable = new CompositeDisposable();
            AmmoInMagazine
                .Subscribe(_ => IsShoot = AmmoInMagazine.Value > 0)
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable?.Clear();
        }
    }
}