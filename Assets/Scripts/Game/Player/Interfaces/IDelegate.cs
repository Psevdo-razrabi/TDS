using System;

namespace Game.Player.Interfaces
{
    public interface IDelegate
    {
        Action OnStartMove { get; }
        Action OnStopMove { get; }
    }
}