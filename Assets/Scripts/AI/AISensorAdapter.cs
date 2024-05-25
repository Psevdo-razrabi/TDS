using UnityEngine;

public class AISensorAdapter : IAISensor
{
    protected GameObject _current, _target;
    protected Vector3 _lastPosition;

    public AISensorAdapter(GameObject current, GameObject target)
    {
        _current = current;
        _target = target;
    }

    virtual public Vector3 GetLastPosition()
    {
        return _lastPosition;
    }

    virtual public bool IsTriggered()
    {
        return false;
    }

}
