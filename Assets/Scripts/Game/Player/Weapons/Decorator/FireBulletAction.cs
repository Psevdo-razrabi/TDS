using Game.Player.Weapons.InterfaceWeapon;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;

namespace Game.Player.Weapons.Decorator
{
    public class FireBulletAction : IAction
    {
        private WeaponData _weaponData;
        private FireStrategy _fireStrategy;

        public FireBulletAction(WeaponData weaponData, FireStrategy fireStrategy)
        {
            _weaponData = weaponData;
            _fireStrategy = fireStrategy;
        }

        public void Execute()
        {
            Debug.LogWarning($"Shooooooooooooooooot {_fireStrategy.GetType()}");
            // ссылка на bullet component?? bullet? bulletComponent
        }
    }
}