using UniRx;
using UnityEngine;

namespace Input.Interface
{
    public interface IMove
    {
        Vector2ReactiveProperty MoveNonInterpolated { get; }
    }
}