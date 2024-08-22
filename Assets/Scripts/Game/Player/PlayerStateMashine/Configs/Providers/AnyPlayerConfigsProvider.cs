using Game.Core.Health;
using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs.Providers
{
    [CreateAssetMenu(fileName = "ConfigProvider", menuName = "ConfigProvider/AnyPlayerProvider")]
    public class AnyPlayerConfigsProvider : ScriptableObject
    {
        [field: SerializeField] public PlayerHealthConfig HealthConfig { get; private set; }
        [field: SerializeField] public PlayerFogOfWarConfig FowConfig { get; private set; }
    }
}