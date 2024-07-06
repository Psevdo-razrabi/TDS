using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/FogOfWarConfig")]
    public class PlayerFogOfWarConfig : ScriptableObject
    {
        [field: SerializeField] public float StartValueRadius { get; private set; }
        [field: SerializeField] public float EndValueRadius { get; private set; }
        [field: SerializeField] public float TimeToMaxRadius { get; private set; }
    }
}