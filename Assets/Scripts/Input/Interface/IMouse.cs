using UniRx;
using UnityEngine;

namespace Input.Interface
{
    public interface IMouse
    {
        Vector2ReactiveProperty PositionMouse { get; }
    }
}