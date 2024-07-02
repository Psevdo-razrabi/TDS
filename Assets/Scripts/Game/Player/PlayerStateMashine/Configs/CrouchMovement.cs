using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/CrouchMovement")]
    public class CrouchMovement : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public AnimationCurve CurveToMaxSpeed { get; private set; }
        [field: SerializeField] public float TimeToMaxSpeed { get; private set; }
    }
}