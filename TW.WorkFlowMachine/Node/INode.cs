namespace TW.WorkFlowMachine.Node
{
    public interface INode<T>
    {
        void Process(T obj);
        string Name { get; }
    }
}