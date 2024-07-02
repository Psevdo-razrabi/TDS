using System;
using FMODUnity;
using UnityEngine;

namespace Game.Player.Weapons.AudioWeapon
{
    [Serializable]
    public class AudioWeapon
    {
        [field: SerializeField] public AudioType AudioType { get; private set; }
        [field: SerializeField] public EventReference AudioClip { get; private set; }
    }
}