using System.Linq;
using FMODUnity;
using Game.Player.Weapons.AudioWeapon;
using UnityEngine;
using Weapons.InterfaceWeapon;
using AudioType = Game.Player.Weapons.AudioWeapon.AudioType;

namespace Game.Player.Weapons
{
    public class AudioComponent : IAudioWeapon
    {
        private readonly AudioStorage _storage;

        public AudioComponent(AudioStorage storage)
        {
            _storage = storage;
        }

        public void PlayOneShot(AudioType type, Vector3 position)
        {
            RuntimeManager.PlayOneShot(_storage.GetAudiosSounds.FirstOrDefault(audio => audio.AudioType == type)!.AudioClip, position);
        }
    }
    
}