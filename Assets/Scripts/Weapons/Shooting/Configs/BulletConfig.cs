using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig",menuName = "ShootConfig/new BulletConfig")]
public class BulletConfig : ScriptableObject
{
    [field: SerializeField] public GameObject BulletPrefab { get; private set; } 
    [field: SerializeField] public float BulletSpeed { get; private set; }
}
