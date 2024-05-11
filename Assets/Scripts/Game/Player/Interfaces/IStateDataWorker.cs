using Game.Player.PlayerStateMashine;
using UI.Storage;
using UniRx;

namespace Game.Player.Interfaces
{
    public interface IStateDataWorker
    {
        PlayerConfigs PlayerConfigs { get;  }
        StateMachineData StateMachineData { get; }
        ValueCountStorage<int> ValueModelDash { get; }
    }
}