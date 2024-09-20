using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BehaviourTree
{
    public class BTDebugger : IBTDebugger
    {
        public ReactiveCollection<string> NameNode { get; private set; } = new(new List<string>());
        public ReactiveCollection<string> TypeNode { get; private set; } = new(new List<string>());
        public ReactiveProperty<BTNodeStatus> NodeStatus { get; private set; } = new();

        private CompositeDisposable _compositeDisposable = new();


        public string GetStatusDebug(BTNodeStatus btNodeStatus)
        {
            return $"{btNodeStatus}";
        }

        public List<string> GetNameNode()
        {
            return NameNode.ToList();
        }

        public List<string> GetTypeNode()
        {
            return TypeNode.ToList();
        }
    }
}