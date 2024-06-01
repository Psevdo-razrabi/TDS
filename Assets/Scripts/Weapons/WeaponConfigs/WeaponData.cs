using UniRx;
using Zenject;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class WeaponData
    {
        public bool IsReloading { get; set; }
        public ReactiveProperty<int> AmmoInMagazine { get; set; } 
    }
}