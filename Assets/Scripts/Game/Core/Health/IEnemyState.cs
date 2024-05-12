namespace Game.Core.Health
{
    public interface IEnemyState
    {
        bool IsHealthRestoring { get; set; }
        bool IsEnemyDie { get; set; }
    }
}