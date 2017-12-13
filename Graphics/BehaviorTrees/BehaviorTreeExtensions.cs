using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.BehaviorTrees
{
    public enum BehaviorStatuses
    {
        Dormant,
        Running,
        Success,
        Failure
    }

    public static class BehaviorTreeExtensions
    {
        /// <summary>
        /// Determines if the node is complete or not.
        /// </summary>
        /// <param name="status">The behavior status</param>
        /// <returns>Returns true if the status is either Success or Failure.</returns>
        public static bool IsComplete(this BehaviorStatuses status) => status == BehaviorStatuses.Success || status == BehaviorStatuses.Failure;
    }
}
