namespace Game.Player.Weapons.InterfaseWeapon
{
    public interface IFireMediator
    {
        void ChangeFireMode(IFireStrategy fireMediator);
    }
}