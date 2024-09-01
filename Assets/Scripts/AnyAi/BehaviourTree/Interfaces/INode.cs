
namespace BehaviourTree
{
    public interface INode
    {
        BTNodeStatus Status { get; }
        string Name { get; }
        int CurrentChild { get; }
        int Priority { get; }
        BTNodeStatus Process();
        void Reset();
    }
}