namespace TW.WorkFlowMachine.Node
{
    public abstract class SplitNode<T> : INode<T>
    {
        private readonly string name;
        private readonly INode<T> trueNextNode;
        private readonly INode<T> falseNextNode;

        protected SplitNode(string name, INode<T> trueNextNode, INode<T> falseNextNode)
        {
            this.name = name;
            this.trueNextNode = trueNextNode;
            this.falseNextNode = falseNextNode;
        }

        public void Process(T obj)
        {
            if (Condition(obj))
                trueNextNode.Process(obj);
            else
                falseNextNode.Process(obj);
        }

        protected abstract bool Condition(T obj);

        public string Name
        {
            get { return name; }
        }
    }
}
