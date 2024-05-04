using UnityEngine;

[CreateAssetMenu(fileName = "GunConfig",menuName = "ShootConfig/new GunConfig")]
public class GunConfig: ScriptableObject
{
    [field: SerializeField] public Transform BulletPoint { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float TimeBetweenShots { get; private set; }
    [field: SerializeField] public float MaxSpread { get; private set; }
    [field: SerializeField] public float CountBullet { get; private set; }
}

