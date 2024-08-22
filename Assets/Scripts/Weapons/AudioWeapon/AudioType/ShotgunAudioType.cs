namespace Game.Player.Weapons.AudioWeapon
{
    public class ShotgunAudioType : WeaponAudioType
    {
        public override AudioType WeaponShoot() => AudioType.ShotgunShoot;

        public override AudioType WeaponReload() => AudioType.Reload;

        public override AudioType WeaponOutAmmo() => AudioType.OutOfAmmoShoot;
    }
}