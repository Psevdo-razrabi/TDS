using Input.Interface;

namespace Game.Player.Weapons.ChangeWeapon
{
    public class WeaponChange
    {
        private readonly IChangeWeapon _changeWeapon;
        
        public void Change(WeaponComponent weaponComponent)
        {
            _changeWeapon.ChangeWeapon(weaponComponent);
        }
        
        public WeaponChange(IChangeWeapon weaponChange)
        {
            _changeWeapon = weaponChange;
        }
    }
}