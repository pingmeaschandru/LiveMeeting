using System.Collections.Generic;
using TW.WorkFlowMachine.Node;

namespace TW.WorkFlowMachine
{
    public class WorkFlowEngine<T>
    {
        private readonly string name;
        private readonly Dictionary<string, INode<T>> nodes;
        private readonly INode<T> startNode;

        public WorkFlowEngine(string name, string startNodeName, IEnumerable<INode<T>> nodes)
        {
            this.name = name;
            this.nodes = new Dictionary<string, INode<T>>();
            foreach (var node in nodes)
            {
                if (node.Name == startNodeName)
                    startNode = node;

                this.nodes.Add(node.Name, node);
            }
        }

        public void Process(T obj)
        {
            startNode.Process(obj);
        }

        public string Name
        {
            get { return name; }
        }
    }
}