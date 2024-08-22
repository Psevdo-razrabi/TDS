using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs.Providers
{
    [CreateAssetMenu(fileName = "ConfigProvider", menuName = "ConfigProvider/CrouchProvider")]
    public class CrouchConfigsProvider : ScriptableObject
    {
        [field: SerializeField] public CrouchAndStandConfig StandUpCrouch { get; private set; }
        [field: SerializeField] public CrouchAndStandConfig SitDownCrouch { get; private set; }
        [field: SerializeField] public CrouchMovement CrouchMovement { get; private set; }
        [field: SerializeField] public AnyCrouchParameters CrouchParameters { get; private set; }
    }
}