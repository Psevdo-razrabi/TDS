using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemy.interfaces
{
    public interface IDie<T>
    {
        UniTask Died();
    }
}