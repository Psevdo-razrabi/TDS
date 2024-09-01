using System.Text;
using UniRx;

namespace BehaviourTree
{
    public interface IBTDebugger
    {
        ReactiveProperty<string> NameNode { get; }
        ReactiveProperty<string> TypeNode { get; }
        ReactiveProperty<BTNodeStatus> NodeStatus { get; }

        string GetStatusDebug(BTNodeStatus btNodeStatus);

        string GetNameNode(string nameNode);

        string GetTypeNode<T>(T typeNode);
    }
}