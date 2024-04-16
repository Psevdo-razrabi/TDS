using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Dash")]
    public class PlayerDashConfig : ScriptableObject
    {
        [field: SerializeField] public float DashDistance { get; private set; }
        [field: SerializeField] public LayerMask LayerObstacle { get; private set; }
        [field: SerializeField] public float DashDelay { get; private set; }
        [field: SerializeField] public float NumberChargesDash { get; private set; }
        [field: SerializeField] public float DashDuration { get; private set; }
    }
}