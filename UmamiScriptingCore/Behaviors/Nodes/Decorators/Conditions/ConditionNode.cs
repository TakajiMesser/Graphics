﻿using SpiceEngineCore.Utilities;

namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public abstract class ConditionNode : DecoratorNode
    {
        public ConditionNode(Node child) : base(child) { }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (Condition(context))
            {
                return Child.Tick(context);
            }
            else
            {
                return BehaviorStatus.Failure;
            }
        }

        protected abstract bool Condition(BehaviorContext context);
    }
}
