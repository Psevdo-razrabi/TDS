using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Move")]
    public class PlayerMoveConfig : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float SpeedStrafe { get; private set; }
        [field: SerializeField] public float SpeedBackwards { get; private set; }
        [field: SerializeField] public float SpeedAngleForward { get; private set; }
        [field: SerializeField] public float SpeedAngleBackwards { get; private set; }
        [field: SerializeField] public float TimeInterpolateSpeed { get; private set; }
    }
}