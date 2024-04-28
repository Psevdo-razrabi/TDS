using UnityEngine;
namespace Game.Player.Weapons.WeaponConfigs
{
    [CreateAssetMenu(fileName = "WeaponConfigs", menuName = "WeaponConfigs/Rifle")]
    public class RifleConfig : BaseWeaponConfig
    {
        [field: SerializeField] public float TimeBetweenShoots { get; protected set; }
        [field: SerializeField] public float MaxSpread { get; protected set; }
        [field: SerializeField] public int MaxSpreadBullet { get; protected set; }
        [field: SerializeField] public float RecoilForce { get; protected set; }
    }
}
