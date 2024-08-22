using Cysharp.Threading.Tasks;
using Game.AsyncWorker.Interfaces;
using Game.Player.Weapons;
using Game.Player.Weapons.AudioWeapon;

namespace Weapons.Commands.Recievers
{
    public class InitializeWeaponAudio
    {
        private readonly AudioStorage _storage;

        public InitializeWeaponAudio(AudioStorage storage)
        {
            _storage = storage;
        }

        public void ChangeAudioWeapon(WeaponComponent weaponComponent)
        {
            _storage.AddOrUpdateWeaponSound(weaponComponent);
        }
    }
}