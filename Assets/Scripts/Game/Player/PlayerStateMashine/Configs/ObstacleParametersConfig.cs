using System;
using UnityEngine;

namespace Game.Player.PlayerStateMashine.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/ObstacleParameters")]
    public class ObstacleParametersConfig : ScriptableObject
    {
        [field: Header("Obstacle Height Correction")] [field: SerializeField]
        public RangeObstacle RangeAndCorrectionForClimb { get; private set; }
        
        [field: Header("Obstacle Parameters Animation")]
        [field: SerializeField] public AvatarTarget AvatarTargetForClimb { get; private set; }
        [field: SerializeField] public Vector3 WeightMask { get; private set; }
        [field: SerializeField] public RangeAnimationClip AnimationClip { get; private set; }
    }
    
    [Serializable]
    public struct RangeObstacle
    {
        public float From;
        public float Before;
        public float HeightCorrection;
    }

    [Serializable]
    public struct RangeAnimationClip
    {
        public float start;
        public float end;
    }
}