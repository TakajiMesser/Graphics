namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public class SelectionConditionNode : ConditionNode
    {
        public int EntityID { get; }

        public SelectionConditionNode(Node child, int entityID = 0) : base(child) => EntityID = entityID;

        protected override bool Condition(BehaviorContext context)
        {
            var entityID = context.GetEntityIDFromMouseCoordinates();

            if (EntityID == 0)
            {
                if (entityID == context.Entity.ID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return entityID == EntityID;
            }
        }
    }
}
