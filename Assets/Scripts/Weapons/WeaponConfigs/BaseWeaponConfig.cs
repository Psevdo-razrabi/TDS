using UniRx;
using UnityEngine;
namespace Game.Player.Weapons.WeaponConfigs
{
    public class BaseWeaponConfig : ScriptableObject
    { 
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField, Range(0,100)] public int TotalAmmo { get; protected set; }
        
        [field: SerializeField, Range(0,10)] public float TimeBetweenShoots { get; protected set; }
        
        [field: Header("Настройки разброса")]
        [field: SerializeField, Range(0,100)] public float StartSpread { get; protected set; }
        [field: SerializeField, Range(0,100)] public float MaxSpread { get; protected set; }
        [field: SerializeField, Range(0,2)] public float TimeToSpreadReduce { get; protected set; }
        [field: SerializeField, Range(0,2)] public float SpreadMultiplier { get; protected set; }
        [field: SerializeField, Range(0,1)] public float MultiplierIncreaseRate { get; protected set; }
        [field: SerializeField, Range(0,5)] public float BaseIncrement { get; protected set; }
        
        [field: Header("Настройки отдачи")]
        [field: SerializeField, Range(0,100)] public float RecoilForce { get; protected set; }
        
        [field: Header("Перезарядка")]
        [field: SerializeField, Range(0,15)] public float ReloadTime { get; protected set; }
        
        [field: Header("Режимы стрельбы")]
        [field: SerializeField] public bool SingleFire { get; protected set; }
        [field: SerializeField] public bool BurstFire { get; protected set; }
        [field: SerializeField, Range(0,5)] public float BurstReloadTime { get; protected set; }
        [field: SerializeField] public bool AutomaticFire { get; protected set; }
    }
}
