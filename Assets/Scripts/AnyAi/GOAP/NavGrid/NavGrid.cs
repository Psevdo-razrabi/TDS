using ModestTree;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class NavGrid : MonoBehaviour
{
    public Transform _mainPoint;
    public Vector3 _lastMainPosition;
    public GameObject _debugPrefab;
    private NavMeshSurface _navMesh;
    private bool _debug = true;
    private string _debugTag = "NavGridDebugSphere";
    private GameObject _debugText;
    private TextMeshProUGUI _debugTMP;
    private GameObject _debugContainer;
    public PointMap _pointMap;
    private GeneratorChunksRound _generatorChunksRound;

    private HashSet<CIndex> _createdDebugPoints = new HashSet<CIndex>();

    private int _obstaclesLayers = 0;

    public class GridCallback : IGenerateCallback
    {
        private readonly int layer;
        private readonly float maxDistanceToFloor = 1.5F;
        private readonly Collider[] _tempColliders = new Collider[6];
        private Collider[] allColliders = null;

        public GridCallback()
        {
            layer = 1 << LayerMask.NameToLayer("Ground");
        }

        public int GetChunkHeight(Vector2 position, Vector2 size)
        {
            if (allColliders == null)
            {
                allColliders = Physics.OverlapBox(Vector3.zero, new Vector3(int.MaxValue, int.MaxValue, int.MaxValue));
            }
            return 10;
        }

        public bool PointIsPossibleGenerate(Vector3 position, out float distanceToFloor)
        {
            distanceToFloor = -1;
            int countColliders = Physics.OverlapSphereNonAlloc(position, 0.2F, _tempColliders, layer);
            if (countColliders > 0)
            {
                return false;
            }
            Ray ray = new(position, Vector3.down);
            RaycastHit raycastHit = new RaycastHit();
            if (Physics.Raycast(ray, out raycastHit, maxDistanceToFloor, layer))
            {
                distanceToFloor = raycastHit.distance;
                return true;
            }

            return false;
        }

        public bool PointsIsNotVisible(Vector3 positionA, Vector3 positionB)
        {
            Ray ray = new Ray(positionA, positionB - positionA);
            if (Physics.Raycast(ray, (positionB - positionA).magnitude, layer))
            {
                return true;
            }
            return false;
        }
    }

    public NavGrid()
    {

    }

    public void AddObstaclesLayer(int layer)
    {
        _obstaclesLayers = (1 << layer) | _obstaclesLayers;
    }

    public void AddObstaclesLayer(string layer)
    {
        AddObstaclesLayer(LayerMask.NameToLayer(layer));
    }

    public void RemoveObstaclesLayer(int layer)
    {
        _obstaclesLayers = (~(1 << layer)) & _obstaclesLayers;
    }

    public void RemoveObstaclesLayer(string layer)
    {
        RemoveObstaclesLayer(LayerMask.NameToLayer(layer));
    }

    void Awake()
    {
        _pointMap = new PointMap(new GridCallback());

        _generatorChunksRound = new GeneratorChunksRound(_pointMap, _mainPoint, 25);

        if (_debug)
        {
            _debugContainer = GameObject.Find("DebugContainer");
            _debugText = GameObject.Find("Text (TMP)");
            _debugTMP = _debugText.GetComponent<TextMeshProUGUI>();
            _debugTMP.enableAutoSizing = true;
            _debugTMP.enableWordWrapping = true;
        }

        _navMesh = GetComponent<NavMeshSurface>();
        _lastMainPosition = _mainPoint.position;
    }

    void Update()
    {
        _pointMap.Update();
        _generatorChunksRound.Update();

        if (_debug)
        {
            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                var point = _pointMap.GetPointAtPosition(_mainPoint.transform.position);
            }

            var v = _pointMap.GetPointAtPosition(_mainPoint.transform.position + new Vector3(0, 0.0F, 0));
            if (v == null)
            {
                _debugTMP.text = "NULL";
            }
            else
            {
                _debugTMP.text = v.ToString() + "\n" + _pointMap.GetCountGenerateTask() + "\n NVP:" + v.notVisiblePoints.Count;
                foreach (var p in v.notVisiblePoints)
                {
                    UnityEngine.Debug.DrawLine(p.position, v.position, Color.green);
                }

            }
            DrawRects(1);
        }
    }

    private void DrawRects(int r)
    {
        //UnityEngine.Debug.
    }

    private void FixedUpdate()
    {
        if (_debug)
        {
            foreach (CIndex cindex in _pointMap.map.Keys)
            {
                if (!_createdDebugPoints.Contains(cindex) && _pointMap.map[cindex].PointsIsCreated())
                {
                    ChunkCreatePoints(_pointMap.map[cindex]);
                    _createdDebugPoints.Add(cindex);
                }

            }
        }
    }

    private GameObject CreateDebugPoint(Vector3 pos)
    {
        GameObject p = Instantiate(_debugPrefab, _debugContainer.transform, true);
        p.transform.position = pos;
        p.layer = 0;
        p.tag = _debugTag;
        return p;
    }

    private void CreateDebugPoint(Vector3 pos, Color color)
    {
        var p = CreateDebugPoint(pos);
        var m = p.GetComponent<MeshRenderer>();
        m.material.color = color;
    }

    public void ChunkCreatePoints(Chunk chunk)
    {
        foreach (var i in chunk.indices)
        {
            var p = chunk.points.Get(i);
            CreateDebugPoint(p.position);
        }
    }

    public PointMap GetPointMap()
    {
        return _pointMap;
    }
}
