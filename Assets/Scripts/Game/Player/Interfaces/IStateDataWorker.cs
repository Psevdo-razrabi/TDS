using Game.Player.PlayerStateMashine;
using UniRx;

namespace Game.Player.Interfaces
{
    public interface IStateDataWorker
    {
        ReactiveProperty<int> Text1 { get; }
        PlayerConfigs PlayerConfigs { get;  }
        StateMachineData StateMachineData { get; }
    }
}