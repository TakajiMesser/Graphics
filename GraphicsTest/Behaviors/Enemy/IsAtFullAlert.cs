using TakoEngine.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.BehaviorTrees;
using TakoEngine.Entities;
using TakoEngine.Physics.Raycasting;
using System.Runtime.Serialization;
using TakoEngine.Scripting.BehaviorTrees.Decorators;

namespace GraphicsTest.Behaviors.Enemy
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
