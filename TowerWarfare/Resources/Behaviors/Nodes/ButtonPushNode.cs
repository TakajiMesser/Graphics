using StarchUICore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes.Leaves;

namespace TowerWarfare.Resources.Behaviors.Nodes
{
    public class ButtonPushNode : LeafNode
    {
        protected override void Execute(BehaviorContext context)
        {
            if (context.GetComponent<IElement>() is Element element)
            {
                //element.Position = element.Position.Offset(Unit.Pixels(element.Location.X + 10), Unit.Pixels(element.Location.Y + 10));
                element.Measurement.Translate(element.Measurement.X + 10, element.Measurement.Y + 10);
                element.InvokeMeasurementChanged();
                //element.Location.Invalidate();
                //element.Parent.Location.Invalidate();
            }
        }
    }
}
