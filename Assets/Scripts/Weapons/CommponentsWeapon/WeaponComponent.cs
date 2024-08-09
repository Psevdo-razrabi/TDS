using Game.Player.Weapons.AudioWeapon;
using Weapons.InterfaceWeapon;
using Zenject;

namespace Game.Player.Weapons
{
    public abstract class WeaponComponent
    {
        public ReloadComponent ReloadComponent { get; protected set; }
        public FireComponent FireComponent { get; protected set; }
        public AudioComponent AudioComponent { get; protected set; }
        public ParticleComponent ParticleComponent { get; protected set; }
        public abstract WeaponAudioType WeaponAudioType { get; protected set; }
        
        [Inject]
        public void Construct(ReloadComponent reloadComponent, FireComponent fireComponent, AudioComponent audioComponent, ParticleComponent particleComponent)
        {
            ReloadComponent = reloadComponent;
            FireComponent = fireComponent;
            AudioComponent = audioComponent;
            ParticleComponent = particleComponent;
        }
        
        public abstract void Accept(IVisitWeaponType visitor);
    }
}