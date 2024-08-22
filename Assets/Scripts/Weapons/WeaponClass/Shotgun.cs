using Game.Player.Weapons.AudioWeapon;
using Weapons.InterfaceWeapon;
using Zenject;

namespace Game.Player.Weapons.WeaponClass
{
    public sealed class Shotgun : WeaponComponent
    {
        public override WeaponAudioType WeaponAudioType { get; protected set; }
        
        [Inject]
        public void Construct(ShotgunAudioType pistolAudioType)
        {
            WeaponAudioType = pistolAudioType;
        }
        
        public override void Accept(IVisitWeaponType visitor)
        {
            visitor.Visit(this);
        }
    }
}