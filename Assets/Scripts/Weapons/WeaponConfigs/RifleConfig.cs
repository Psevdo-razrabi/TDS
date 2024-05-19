using UnityEngine;
namespace Game.Player.Weapons.WeaponConfigs
{
    [CreateAssetMenu(fileName = "WeaponConfigs", menuName = "WeaponConfigs/Rifle")]
    public class RifleConfig : BaseWeaponConfig
    {
        [field: SerializeField] public float TimeBetweenShoots { get; protected set; }
        
        [field: Header("Настройки разброса")]
        [field: SerializeField] public float MaxSpread { get; protected set; }
        [field: SerializeField] public float MaxSpreadBullet { get; protected set; }
        [field: SerializeField] public float TimeToSpreadReduce { get; protected set; }
        
        [field: SerializeField] public float BaseSpread{ get; protected set; }
        [field: SerializeField] public float GrowthFactor{ get; protected set; }

        
        [field: Header("Настройки отдачи")]
        [field: SerializeField] public float RecoilForce { get; protected set; }
        
        [field: Header("Перезарядка")]
        [field: SerializeField] public float ReloadTime { get; protected set; }
    }
}
