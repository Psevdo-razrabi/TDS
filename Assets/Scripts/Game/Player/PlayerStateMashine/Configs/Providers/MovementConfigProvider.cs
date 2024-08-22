using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs.Providers
{
    [CreateAssetMenu(fileName = "ConfigProvider", menuName = "ConfigProvider/MovementProvider")]
    public class MovementConfigsProvider : ScriptableObject
    {
        [field: SerializeField] public PlayerMoveConfig BaseMove { get; private set; }
        [field: SerializeField] public PlayerMoveConfig MoveWithAim { get; private set; }
        [field: SerializeField] public PlayerDashConfig DashConfig { get; private set; }
        [field: SerializeField] public PlayerParkourConfig ParkourConfig { get; private set; }
    }
}