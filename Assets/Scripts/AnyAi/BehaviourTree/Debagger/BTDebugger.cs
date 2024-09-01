using UniRx;
using UnityEngine;

namespace BehaviourTree
{
    public class BTDebugger : IBTDebugger
    {
        public ReactiveProperty<string> NameNode { get; private set; } = new();
        public ReactiveProperty<string> TypeNode { get; private set; } = new();
        public ReactiveProperty<BTNodeStatus> NodeStatus { get; private set; } = new();

        private CompositeDisposable _compositeDisposable = new();


        public string GetStatusDebug(BTNodeStatus btNodeStatus)
        {
            return $"Status Node: {btNodeStatus}";
        }

        public string GetNameNode(string nameNode)
        {
            return $"Current Node: {NameNode}";
        }

        public string GetTypeNode<T>(T typeNode)
        {
            return $"Current TypeNode: {typeNode}";
        }
    }
}