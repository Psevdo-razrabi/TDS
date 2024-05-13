using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/FillImage")]
    public class PlayerDashConfig : ScriptableObject
    {
        [field: SerializeField] public float DashDistance { get; private set; }
        [field: SerializeField] public LayerMask LayerObstacle { get; private set; }
        [field: SerializeField] public float DashDelay { get; private set; }
        [field: SerializeField] public float DelayAfterEachDash { get; private set; }
        [field: SerializeField] public int NumberChargesDash { get; private set; }
        [field: SerializeField] public float DashDuration { get; private set; }
    }
}