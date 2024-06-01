using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeConfig",menuName = "ShootConfig/new ShakeConfig")]
public class CameraShakeConfig : ScriptableObject
{
    [field: SerializeField] public float ShakeDuration { get; private set; }
    [field: SerializeField] public float ShakeStrength { get; private set; }
}
