using UnityEngine;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class BaseWeaponConfig : ScriptableObject
    {
        [field: SerializeField] public int AmmoInMagazine { get; protected set; }
        [field: SerializeField] public int TotalAmmo { get; protected set; }
        [field: SerializeField] public float ReloadTime { get; protected set; }
    }
} 