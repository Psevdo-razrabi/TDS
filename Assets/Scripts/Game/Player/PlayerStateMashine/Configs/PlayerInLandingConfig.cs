using System;
using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Landing")]
    public class PlayerInLandingConfig : ScriptableObject
    {
        [field: SerializeField] public RaycastParameters ClambingObject { get; private set; }
        [field: SerializeField] public RaycastParameters LandingObject { get; private set; }
        [field: SerializeField] public RaycastParameters Ground { get; private set; }
    }


    [Serializable]
    public class RaycastParameters
    {
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
        [field: SerializeField] public float CastDistance { get; private set; }
    }
}