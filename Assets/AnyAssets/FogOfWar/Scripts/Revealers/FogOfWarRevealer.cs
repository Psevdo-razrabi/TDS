using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace FOW
{
    public abstract class FogOfWarRevealer : MonoBehaviour
    {
        [Header("Customization Variables")]
        [SerializeField] protected float ViewRadius = 15;
        [SerializeField] protected float AdditionalSoftenDistance = 0;

        [Range(0, 360)] [SerializeField] public float ViewAngle;
        [SerializeField] protected float unobscuredRadius = 1f;

        [SerializeField] protected bool addCorners;

        [SerializeField] protected bool revealHidersInFadeOutZone = true;

        [Tooltip("how high above this object should the sight be calculated from")]
        [SerializeField] protected float EyeOffset = 0;
        [SerializeField] protected float VisionHeight = 3;
        [SerializeField] protected float VisionHeightSoftenDistance = 1.5f;

        [Header("Technical Variables")]
        [SerializeField] protected LayerMask ObstacleMask;
        [SerializeField] protected float RaycastResolution = .5f;

        [Range(1, 30)]
        [Tooltip("Higher values will lead to more accurate edge detection, especially at higher distances. however, this will also result in more raycasts.")]
        [SerializeField] protected int MaxEdgeResolveIterations = 10;

        [Range(0, 10)]
        [SerializeField] protected int NumExtraIterations = 4;

        [Range(0, 5)]
        [SerializeField] protected int NumExtraRaysOnIteration = 5;

        [HideInInspector]
        public int FogOfWarID;
        [HideInInspector]
        public int IndexID;

        //local variables
        //protected List<ViewCastInfo> ViewPoints = new List<ViewCastInfo>();
        protected FogOfWarWorld.CircleStruct CircleStruct;
        protected bool IsRegistered = false;
        protected ViewCastInfo[] ViewPoints;
        protected int NumberOfPoints;
        public float[] Radii;
        public float[] Distances;
        public bool[] AreHits;

        [Header("debug, you shouldnt have to mess with this")]
        [Range(.001f, 1)]
        [Tooltip("Lower values will lead to more accurate edge detection, especially at higher distances. however, this will also result in more raycasts.")]
        [SerializeField] protected float MaxAcceptableEdgeAngleDifference = .005f;
        [SerializeField] protected float EdgeDstThreshold = 0.1f;
        [SerializeField] protected float DoubleHitMaxDelta = 0.1f;
        [SerializeField] protected float DoubleHitMaxAngleDelta = 15;
#if UNITY_EDITOR
        [SerializeField] protected bool LogNumRaycasts = false;
        [SerializeField] protected bool DebugMode = false;
        [SerializeField] protected int NumRayCasts;
        [SerializeField] protected float DrawRayNoise = 0;
        [SerializeField] protected bool DrawExtraCastLines;
        [SerializeField] protected bool DrawIteritiveLines;
#endif
        public ReactiveCollection<FogOfWarHider> hidersSeen { get; } = new();

        public struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;
            public Vector3 normal;
            public Vector3 direction;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle, Vector3 _normal, Vector3 dir)
            {
                hit = _hit;
                point = _point;
                dst = _dst;
                angle = _angle;
                normal = _normal;
                direction = dir;
            }
        }

        public struct EdgeInfo
        {
            public ViewCastInfo minViewCast;
            public ViewCastInfo maxViewCast;
            public bool shouldUse;

            public EdgeInfo(ViewCastInfo _pointA, ViewCastInfo _pointB, bool _shouldUse)
            {
                minViewCast = _pointA;
                maxViewCast = _pointB;
                shouldUse = _shouldUse;
            }
        }

        private void OnEnable()
        {
            ViewPoints = new ViewCastInfo[FogOfWarWorld.instance.maxPossibleSegmentsPerRevealer];

            Radii = new float[ViewPoints.Length];
            Distances = new float[ViewPoints.Length];
            AreHits = new bool[ViewPoints.Length];
            RegisterRevealer();
        }

        private void OnDisable()
        {
            DeregisterRevealer();
        }
        public void RegisterRevealer()
        {
            NumberOfPoints = 0;
            if (FogOfWarWorld.instance == null)
            {
                if (!FogOfWarWorld.revealersToRegister.Contains(this))
                {
                    FogOfWarWorld.revealersToRegister.Add(this);
                }
                return;
            }
            if (IsRegistered)
            {
                Debug.Log("Tried to double register revealer");
                return;
            }
            IsRegistered = true;
            FogOfWarID = FogOfWarWorld.instance.RegisterRevealer(this);
            CircleStruct = new FogOfWarWorld.CircleStruct();
            CalculateLineOfSight();
            //_RegisterRevealer();
        }
        public void DeregisterRevealer()
        {
            if (FogOfWarWorld.instance == null)
            {
                if (FogOfWarWorld.revealersToRegister.Contains(this))
                {
                    FogOfWarWorld.revealersToRegister.Remove(this);
                }
                return;
            }
            if (!IsRegistered)
            {
                //Debug.Log("Tried to de-register revealer thats not registered");
                return;
            }
            foreach (FogOfWarHider hider in hidersSeen)
            {
                hider.removeSeer(this);
            }
            hidersSeen.Clear();
            IsRegistered = false;
            FogOfWarWorld.instance.DeRegisterRevealer(this);
        }

        public void CalculateLineOfSight()
        {
            NumberOfPoints = 0;
            _CalculateLineOfSight();
        }
        protected abstract void _CalculateLineOfSight();

        protected void AddViewPoint(ViewCastInfo point)
        {
            ViewPoints[NumberOfPoints] = point;
            NumberOfPoints++;
        }

        public bool TestPoint(Vector3 point)
        {
            return _TestPoint(point);
        }
        protected abstract bool _TestPoint(Vector3 point);
    }
}
