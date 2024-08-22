using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/PlayerParkour")]
    public class PlayerParkourConfig : ScriptableObject
    {
        [field: Header("Layer Settings")]
        [field: SerializeField] public LayerMask LayerMask { get; private set; }

        [field: Header("Raycast Settings")] 
        [field: SerializeField] public float RayCastDistance { get; private set; } 
    }
}