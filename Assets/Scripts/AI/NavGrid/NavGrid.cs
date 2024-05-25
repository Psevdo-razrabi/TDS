using ModestTree;
using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public Transform _mainPoint;
    public Vector3 _lastMainPosition;
    public GameObject _debugPrefab;
    private NavMeshSurface _navMesh;
    private List<GridPoint> _navGrid = new();
    private static float _pointDist = 2f;
    private static  float _generateDistance = 60f;
    private float _regenDistance = MathF.Pow(200, 2);
    private bool _debug = false;
    private string _debugTag = "NavGridDebugSphere";
    private static float _nearestDistance = Mathf.Pow(Mathf.Sqrt(_pointDist * _pointDist + _pointDist * _pointDist)/2, 2);

    void Start()
    {
        _navMesh = GetComponent<NavMeshSurface>();
        _lastMainPosition = _mainPoint.position;
        CreateGrid();
    }

    void Update()
    {
        if (_debug)
        {
            foreach (GridPoint p in _navGrid)
            {
                if ((p._position - _mainPoint.position).sqrMagnitude < 1)
                {
                    foreach (GridPoint gp in p._pointsNotVisible)
                    {
                        Debug.DrawLine(p._position, gp._position);
                    }
                    break;
                }
            }
        }

        if((_mainPoint.position - _lastMainPosition).sqrMagnitude > _regenDistance)
        {
            _lastMainPosition = _mainPoint.position;
            RegenNavGrid();
        }

    }

    private void RegenNavGrid()
    {
        
        CreateGrid();
    }

    private void CreateGrid()
    {

        if (_navMesh == null)
        {
            throw new Exception("NavMeshSurface == null");
        }
        if (_debug)
        {
            GameObject[] sphereObjects = GameObject.FindGameObjectsWithTag(_debugTag);
            if (sphereObjects != null)
            {
                foreach (GameObject so in sphereObjects)
                {
                    GameObject.Destroy(so);
                }
            }
        }
        _navGrid.Clear();
        for (float x = _mainPoint.position.x - _generateDistance / 2; x < _mainPoint.position.x + _generateDistance / 2; x += _pointDist)
        {
            for (float z = _mainPoint.position.z - _generateDistance / 2; z < _mainPoint.position.z + _generateDistance / 2; z += _pointDist)
            {
                Vector3 pos = new(x, 0, z);
                GridPoint gridPoint = new()
                {
                    _position = pos
                };
                if (_debug)
                {
                    CreateDebugPoint(pos);
                }
                CalculateVisiblePoints(gridPoint);
                _navGrid.Add(gridPoint);
            }
        }
    }

    private void CreateDebugPoint(Vector3 pos)
    {
        GameObject p = Instantiate(_debugPrefab, transform, true);
        p.transform.position = pos;
        p.layer = 0;
        p.tag = _debugTag;
    }

    private void CalculateVisiblePoints(GridPoint gridPoint)
    {
        Ray ray = new Ray();
        ray.origin = gridPoint._position;
        int layer = 1 << LayerMask.NameToLayer("Ground");
        foreach (GridPoint p in _navGrid)
        {
            Vector3 dir = p._position - gridPoint._position;
            dir.y = 0;
            ray.direction = dir;
            Collider[] colliders = Physics.OverlapSphere(p._position, 0.2F, layer);
            if (colliders == null || colliders.IsEmpty())
            {
                if (Physics.Raycast(ray, Vector3.Distance(p._position, gridPoint._position), layer))
                {
                    Log.Info("HIT " + ray.ToShortString());
                    gridPoint._pointsNotVisible.Add(p);
                    p._pointsNotVisible.Add(gridPoint);
                }
            }
        }
    }

    public Vector3 FindHiddenMinPosition(Vector3 position)
    {
        GridPoint gridPoint = FindNearestPoint(position);
        if(gridPoint == null || gridPoint._pointsNotVisible.IsEmpty())
        {
            return new(0, 0, 0);
        }
        GridPoint nearestPoint = FindNearestPointMinDistane(position, gridPoint._pointsNotVisible);
        return nearestPoint._position;
    }

    private GridPoint FindNearestPointMinDistane(Vector3 position, List<GridPoint> pointsNotVisible)
    {
        float dist2 = float.MaxValue;
        GridPoint res = new GridPoint();
        foreach(GridPoint point in pointsNotVisible)
        {
            float d2 = (point._position - position).sqrMagnitude;
            if(d2 < dist2)
            {
                dist2 = d2;
                res = point;
            }
        }
        return res;
    }

    private GridPoint FindNearestPoint(Vector3 position)
    {
        foreach(GridPoint p in _navGrid)
        {
            if((position - p._position).sqrMagnitude < _nearestDistance)
            {
                return p;
            }
        }
        return null;
    }
}
