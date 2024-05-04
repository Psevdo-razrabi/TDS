using Game.Player.Weapons.InterfaceWeapon;

namespace Game.Player.Weapons.InterfaseWeapon
{
    public interface IFireMediator
    {
        void ChangeFireMode(FireStrategy fireMediator);
    }
}