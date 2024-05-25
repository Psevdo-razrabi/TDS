using UnityEngine;

public interface IAISensor
{
    bool IsTriggered();
    Vector3 GetLastPosition();
}