using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Weapons.AudioWeapon
{
    public class WeaponAudio : MonoBehaviour
    {
        [field: SerializeField] private List<AudioWeapon> _audioWeapons;

        public IReadOnlyList<AudioWeapon> AudioWeapons => _audioWeapons;
    }
}