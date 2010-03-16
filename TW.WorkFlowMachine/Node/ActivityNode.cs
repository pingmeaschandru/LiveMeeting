namespace TW.WorkFlowMachine.Node
{
    public abstract class ActivityNode<T> : INode<T>
    {
        private readonly string name;
        private readonly INode<T> nextNode;

        protected ActivityNode(string name, INode<T> nextNode)
        {
            this.name = name;
            this.nextNode = nextNode;
        }

        public void Process(T obj)
        {
            Action(obj);
            nextNode.Process(obj);
        }

        protected abstract void Action(T obj);

        public string Name
        {
            get { return name; }
        }
    }
}
