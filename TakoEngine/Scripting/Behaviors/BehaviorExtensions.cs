using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.Behaviors
{
    public enum BehaviorStatus
    {
        Dormant,
        Running,
        Success,
        Failure
    }

    public static class BehaviorExtensions
    {
        public static bool IsComplete(this BehaviorStatus status) => status == BehaviorStatus.Success || status == BehaviorStatus.Failure;
    }
}
