using UniRx;
using Vector3 = UnityEngine.Vector3;

namespace GOAP
{
    public interface ISensor
    {
        Vector3? Target { get; }
        ReactiveProperty<bool> IsActivate { get; }
    }
}