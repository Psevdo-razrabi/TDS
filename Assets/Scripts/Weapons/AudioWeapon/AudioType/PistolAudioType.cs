namespace Game.Player.Weapons.AudioWeapon
{
    public class PistolAudioType : WeaponAudioType
    {
        public override AudioType WeaponShoot() => AudioType.PistolShoot;

        public override AudioType WeaponReload() => AudioType.Reload;

        public override AudioType WeaponOutAmmo() => AudioType.OutOfAmmoShoot;
    }
}