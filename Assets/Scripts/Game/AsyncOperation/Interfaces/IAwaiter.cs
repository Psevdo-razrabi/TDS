using Cysharp.Threading.Tasks;

namespace Game.AsyncWorker.Interfaces
{
    public interface IAwaiter
    {
        UniTask AwaitLoadConfigs(ILoadable load);
    }
}