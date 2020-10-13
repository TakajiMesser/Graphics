using TangyHIDCore;

namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public class SelectionConditionNode : ConditionNode
    {
        public int EntityID { get; }

        public SelectionConditionNode(Node child, int entityID = 0) : base(child) => EntityID = entityID;

        protected override bool Condition(BehaviorContext context)
        {
            var coordinates = context.SystemProvider.GetGameSystem<IInputProvider>().MouseCoordinates;
            
            if (coordinates.HasValue)
            {
                var entityID = context.SystemProvider.RenderProvider.GetEntityIDFromSelection(coordinates.Value);
                return EntityID == 0
                    ? entityID == context.GetEntity().ID
                    : entityID == EntityID;
            }

            return false;
        }
    }
}
