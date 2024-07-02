using UnityEngine;
using AudioType = Game.Player.Weapons.AudioWeapon.AudioType;

namespace Weapons.InterfaceWeapon
{
    public interface IAudioWeapon
    {
        void PlayOneShot(AudioType type, Vector3 position);
    }
}