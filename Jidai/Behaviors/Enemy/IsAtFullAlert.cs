using System.Runtime.Serialization;
using TakoEngine.Scripting.Behaviors;
using TakoEngine.Scripting.Behaviors.Decorators;

namespace Jidai.Behaviors.Enemy
{
    [DataContract]
    public class IsAtFullAlert : ConditionNode
    {
        [DataMember]
        public float FullAlertTicks { get; set; }

        public IsAtFullAlert(int fullAlertTicks, Node node) : base(node)
        {
            FullAlertTicks = fullAlertTicks;
        }

        public override bool Condition(BehaviorContext context)
        {
            int nAlertTicks = context.GetVariableOrDefault<int>("nAlertTicks");

            if (nAlertTicks > FullAlertTicks)
            {
                return true;
            }
            else
            {
                nAlertTicks++;
                context.SetVariable("nAlertTicks", nAlertTicks);
                return false;
            }
        }
    }
}
