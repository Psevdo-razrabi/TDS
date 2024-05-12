namespace Game.Player.Weapons.InterfaseWeapon
{
    public interface IWeapon
    {
        ReloadComponent reloadComponent { get; }
        FireComponent fireComponent { get; }
    }
}