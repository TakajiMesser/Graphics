using SpiceEngineCore.Entities.UserInterfaces;
using StarchUICore;
using StarchUICore.Attributes.Units;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes.Leaves;

namespace TowerWarfare.Resources.Behaviors.Nodes
{
    public class ButtonPushNode : LeafNode
    {
        protected override void Execute(BehaviorContext context)
        {
            if (context.Entity is IUIItem item)
            {
                if (context.GetUIElement(item.ID) is Element element)
                {
                    //element.Position = element.Position.Offset(Unit.Pixels(element.Location.X + 10), Unit.Pixels(element.Location.Y + 10));
                    element.Location.SetValue(element.Location.X + 10, element.Location.Y + 10);
                    element.InvokeLayoutChange();
                    //element.Location.Invalidate();
                    //element.Parent.Location.Invalidate();
                }
            }
        }
    }
}
