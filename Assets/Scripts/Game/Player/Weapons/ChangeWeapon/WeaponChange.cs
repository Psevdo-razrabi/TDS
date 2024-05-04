using Input.Interface;

namespace Game.Player.Weapons.ChangeWeapon
{
    public class WeaponChange
    {
        private readonly IChangeWeapon _changeWeapon;
        private readonly WeaponComponent _weaponComponent;
        
        public void Change()
        {
            _changeWeapon.ChangeWeapon(_weaponComponent);
        }
        
        public WeaponChange(WeaponComponent weaponComponent, IChangeWeapon weaponChange)
        {
            _changeWeapon = weaponChange;
            _weaponComponent = weaponComponent;
        }
    }
}