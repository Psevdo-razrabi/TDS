using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Player.PlayerStateMashine.Configs
{
    public class StorageAssetReference : MonoBehaviour
    {
        [field: SerializeField] public AssetLabelReference PlayerScriptableObjectLabel { get; private set; }
    }
}