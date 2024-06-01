using Game.Player.Weapons;
using Game.Player.Weapons.WeaponClass;

namespace Weapons.InterfaceWeapon
{
    public interface IVisitWeaponType
    {
        void Visit(Pistol pistol);
        void Visit(Rifle rifle);
        void Visit(Shotgun shotgun);
        void VisitWeapon(WeaponComponent component);
    }
}