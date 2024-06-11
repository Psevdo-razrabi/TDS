using UniRx;

namespace CharacterOrEnemyEffect.Interfaces
{
    public interface IIsTrailActive
    {
        ReactiveProperty<bool> IsTrailActive { get; }
    }
}