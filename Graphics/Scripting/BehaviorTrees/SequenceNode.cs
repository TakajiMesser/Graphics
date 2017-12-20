using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees
{
    /// <summary>
    /// A composite node is a node that can have one or more children.
    /// They will process one or more of these children in either a first to last sequence
    /// or random order depending on the particular composite node in question.
    /// </summary>
    public class SequenceNode : INode
    {
        public BehaviorStatuses Status { get; private set; }
        public List<INode> ChildNodes { get; } = new List<INode>();

        public SequenceNode(IEnumerable<INode> nodes)
        {
            ChildNodes.AddRange(nodes);
        }

        public void Tick(Dictionary<string, object> variablesByName)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Running;

                foreach (var node in ChildNodes)
                {
                    if (!Status.IsComplete())
                    {
                        node.Tick(variablesByName);

                        if (node.Status == BehaviorStatuses.Failure)
                        {
                            Status = BehaviorStatuses.Failure;
                            return;
                        }
                        else if (node.Status == BehaviorStatuses.Running)
                        {
                            return;
                        }
                    }
                }

                Status = BehaviorStatuses.Success;
            }
        }

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;

            foreach (var node in ChildNodes)
            {
                node.Reset();
            }
        }
    }
}
