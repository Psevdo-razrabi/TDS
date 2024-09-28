using UnityEngine;

namespace GOAP
{
    public interface ISensorTriggerCommander
    {
        void SetTarget(Vector3 target);
        void SetIsTargetTrigger(bool isTargetDetected);
    }
}