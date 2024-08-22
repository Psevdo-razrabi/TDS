using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/CrouchAndStand")]
    public class CrouchAndStandConfig : ScriptableObject
    {
        [field: SerializeField] public float HeightOfCharacterController { get; private set; }
        [field: SerializeField] public Vector3 CenterCharacterController { get; private set; }
        [field: SerializeField] public AnimationCurve CurveToCrouch { get; private set; }
        [field: SerializeField] public float TimeToCrouch { get; private set; }
    }
}