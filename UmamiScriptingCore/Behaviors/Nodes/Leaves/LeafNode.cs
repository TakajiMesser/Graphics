namespace UmamiScriptingCore.Behaviors.Nodes.Leaves
{
    public abstract class LeafNode : Node
    {
        public override BehaviorStatus Tick(BehaviorContext context)
        {
            Execute(context);
            return BehaviorStatus.Success;
        }

        protected abstract void Execute(BehaviorContext context);

        public override void Reset() { }
    }
}
