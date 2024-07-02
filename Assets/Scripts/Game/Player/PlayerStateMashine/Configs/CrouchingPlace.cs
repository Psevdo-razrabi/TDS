using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/FindPlaceToCrouch")]
    public class CrouchingPlace : ScriptableObject
    {
        [field: SerializeField] public float DistanceToCrouchPlace { get; private set; }
        [field: SerializeField] public LayerMask LayerMaskObjectToCrouch { get; private set; }
    }
}