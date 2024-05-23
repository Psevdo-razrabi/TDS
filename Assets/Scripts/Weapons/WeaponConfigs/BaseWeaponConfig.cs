using UniRx;
using UnityEngine;
namespace Game.Player.Weapons.WeaponConfigs
{
    public class BaseWeaponConfig : ScriptableObject
    {
        [field: SerializeField] public int TotalAmmo { get; protected set; }
        [field: SerializeField] public GameObject BulletPoint { get; protected set; }
        
        [field: SerializeField] public float TimeBetweenShoots { get; protected set; }
        
        [field: Header("Настройки разброса")]
        [field: SerializeField] public float MaxSpread { get; protected set; }
        [field: SerializeField] public int MaxSpreadBullet { get; protected set; }
        [field: SerializeField] public float TimeToSpreadReduce { get; protected set; }
        
        [field: SerializeField] public float SpreadMultiplier { get; protected set; }
        [field: SerializeField] public float MultiplierIncreaseRate { get; protected set; }
        [field: SerializeField] public float BaseIncrement { get; protected set; }
        
        [field: Header("Настройки отдачи")]
        [field: SerializeField] public float RecoilForce { get; protected set; }
        
        [field: Header("Перезарядка")]
        [field: SerializeField] public float ReloadTime { get; protected set; }
    }
}
