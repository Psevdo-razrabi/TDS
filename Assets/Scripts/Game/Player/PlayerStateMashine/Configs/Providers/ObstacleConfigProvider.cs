using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs.Providers
{
    [CreateAssetMenu(fileName = "ConfigProvider", menuName = "ConfigProvider/ObstacleProvider")]
    public class ObstacleConfigsProvider : ScriptableObject
    {   
        [field: SerializeField] public ObstacleParametersConfig SmallObstacle { get; private set; }
        [field: SerializeField] public ObstacleParametersConfig MiddleObstacle { get; private set; }
        [field: SerializeField] public ObstacleParametersConfig LargeObstacle { get; private set; }
        [field: SerializeField] public PlayerInLandingConfig LandingConfig { get; private set; }
    }
}