using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class WeaponData
    {
        public bool IsReloading { get; set; }
        public ReactiveProperty<int> AmmoInMagazine { get; set; } 
        public Transform BulletPoint { get; set; }
    }
}