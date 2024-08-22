using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Enemy.interfaces
{
    public interface IDie
    {
        Subject<Unit> DieAction { get; }
        UniTask Died();
    }
}