using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Player.PlayerStateMashine.Configs
{
    public class StorageAssetReference : MonoBehaviour
    {
        [field: SerializeField] public AssetLabelReference PlayerScriptableObjectLabel { get; private set; }
        [field: SerializeField] public AssetReferenceT<PlayerMoveConfig> PlayerMove { get; private set; }
        [field: SerializeField] public AssetReferenceT<PlayerMoveConfig> PlayerMoveInAim { get; private set; }
        [field: SerializeField] public AssetReferenceT<CrouchAndStandConfig> StandUpCrouch { get; private set; }
        [field: SerializeField] public AssetReferenceT<CrouchAndStandConfig> SitDownCrouch { get; private set; }
        [field: SerializeField] public AssetReferenceT<ObstacleParametersConfig> SmallObstacle { get; private set; }
        [field: SerializeField] public AssetReferenceT<ObstacleParametersConfig> MiddleObstacle { get; private set; }
        [field: SerializeField] public AssetReferenceT<ObstacleParametersConfig> LargeObstacle { get; private set; }
    }
}