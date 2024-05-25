using UnityEngine;

public class AISensorSound : AISensorAdapter
{
    private float _distance;
    private Transform _transformTarget, _transformCurrent;
    public AISensorSound(GameObject current, GameObject target, float distance) : base(current, target)
    {
        _distance = distance * distance;
        _transformCurrent = current.transform;
        _transformTarget = target.transform;
    }

    public override bool IsTriggered()
    {
        float dst = Vector3.SqrMagnitude(_transformCurrent.position - _transformTarget.position);
        if (dst < _distance)
        {
            _lastPosition = _transformTarget.position;
            return true;
        }
        return false;
    }
}
