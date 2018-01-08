using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using Graphics.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Scripting.BehaviorTrees
{
    /// <summary>
    /// The base class that all behavior tree nodes inherit from.
    /// </summary>
    [DataContract]
    [KnownType(typeof(SelectorNode))]
    [KnownType(typeof(SequenceNode))]
    [KnownType(typeof(ParallelNode))]
    [KnownType(typeof(RepeaterNode))]
    [KnownType(typeof(InverterNode))]
    [KnownType(typeof(LoopNode))]
    [KnownType(typeof(NavigateNode))]
    [KnownType(typeof(ConditionNode))]
    public abstract class Node
    {
        public BehaviorStatuses Status { get; set; }

        public abstract void Tick(BehaviorContext context);

        public virtual void Reset(bool recursive = false)
        {
            Status = BehaviorStatuses.Dormant;
        }

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static Node Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as Node;
            }
        }
    }
}
