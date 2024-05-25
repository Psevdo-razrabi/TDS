using UnityEngine;

public class AISensorVision : AISensorAdapter
{
    private float _angle;
    private float _distance;
    private int _countRays;
    private Transform _currTransform;
    private Transform _targetTransform;

    public AISensorVision(GameObject current, GameObject target, float angle, float distance, int countRays) : base(current, target)
    {
        _angle = angle;
        _distance = distance;
        _currTransform = _current.transform;
        _targetTransform = target.transform;
        _countRays = countRays;
    }

    public override bool IsTriggered()
    {
        return false;
    }
}
