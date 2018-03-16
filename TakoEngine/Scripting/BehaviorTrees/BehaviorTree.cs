using System.Runtime.Serialization;
using System.Xml;
using TakoEngine.Scripting.BehaviorTrees.Composites;
using TakoEngine.Scripting.BehaviorTrees.Decorators;
using TakoEngine.Scripting.BehaviorTrees.Leaves;

namespace TakoEngine.Scripting.BehaviorTrees
{
    [DataContract]
    [KnownType(typeof(SelectorNode))]
    [KnownType(typeof(SequenceNode))]
    [KnownType(typeof(ParallelNode))]
    [KnownType(typeof(RepeaterNode))]
    [KnownType(typeof(InverterNode))]
    [KnownType(typeof(LoopNode))]
    [KnownType(typeof(ConditionNode))]
    [KnownType(typeof(InlineLeafNode))]
    [KnownType(typeof(LeafNode))]
    public class BehaviorTree
    {
        public BehaviorStatuses Status { get; private set; }
        public BehaviorContext Context { get; set; } = new BehaviorContext();

        [DataMember]
        public Node RootNode { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext c)
        {
            Context = new BehaviorContext();
        }

        public void Tick()
        {
            RootNode.Tick(Context);
            Status = RootNode.Status;

            if (RootNode.Status.IsComplete())
            {
                RootNode.Reset();
            }
        }

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;
            RootNode.Reset();
        }

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static BehaviorTree Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as BehaviorTree;
            }
        }
    }
}
