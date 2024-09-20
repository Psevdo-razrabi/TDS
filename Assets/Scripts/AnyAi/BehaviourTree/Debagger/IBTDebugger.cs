using System.Collections.Generic;
using System.Text;
using UniRx;

namespace BehaviourTree
{
    public interface IBTDebugger
    {
        ReactiveCollection<string> NameNode { get; }
        ReactiveCollection<string> TypeNode { get; }
        ReactiveProperty<BTNodeStatus> NodeStatus { get; }

        string GetStatusDebug(BTNodeStatus btNodeStatus);

        public List<string> GetNameNode();

        public List<string> GetTypeNode();
    }
}