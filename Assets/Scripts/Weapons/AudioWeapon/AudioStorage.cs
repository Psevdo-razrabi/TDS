using System.Collections.Generic;
using System.Linq;
using Game.Player.Weapons.WeaponClass;
using Weapons.InterfaceWeapon;

namespace Game.Player.Weapons.AudioWeapon
{
    public class AudioStorage : IVisitWeaponType
    {
        private Dictionary<WeaponComponent, List<AudioWeapon>> _audiosSounds = new();
        private WeaponAudio _weaponAudio;
        public IReadOnlyList<AudioWeapon> GetAudiosSounds { get; private set; }

        public AudioStorage(WeaponAudio weaponAudio)
        {
            _weaponAudio = weaponAudio;
        }
        
        public void AddOrUpdateWeaponSound(WeaponComponent component)
        {
            if(CheckContainsKey(component)) return;
        
            component.Accept(this);
        }

        public void Visit(Pistol pistol)
        {
            _audiosSounds.Add(pistol, _weaponAudio.AudioWeapons.Where(x =>
                x.AudioType == AudioType.Reload | x.AudioType == AudioType.PistolShoot |
                x.AudioType == AudioType.OutOfAmmoShoot).ToList());
            GetAudiosSounds = _audiosSounds[pistol];
        }

        public void Visit(Rifle rifle)
        {
            _audiosSounds.Add(rifle,_weaponAudio.AudioWeapons.Where(x =>
                x.AudioType == AudioType.Reload | x.AudioType == AudioType.RifleShoot |
                x.AudioType == AudioType.OutOfAmmoShoot).ToList());
            GetAudiosSounds = _audiosSounds[rifle];
        }

        public void Visit(Shotgun shotgun)
        {
            _audiosSounds.Add(shotgun,_weaponAudio.AudioWeapons.Where(x =>
                x.AudioType == AudioType.Reload | x.AudioType == AudioType.ShotgunShoot |
                x.AudioType == AudioType.OutOfAmmoShoot).ToList());
            GetAudiosSounds = _audiosSounds[shotgun];
        }

        private bool CheckContainsKey(WeaponComponent component)
        {
            var containsKey = _audiosSounds.ContainsKey(component);
            if (containsKey) GetAudiosSounds = _audiosSounds[component];
            return containsKey;
        }
    }
    
}