using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/AnyCrouchParameters")]
    public class AnyCrouchParameters : ScriptableObject
    {
        [field: SerializeField] public float TimeToReloadCrouchState { get; private set; }
    }
}