using Graphics.Helpers;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Scripting.BehaviorTrees
{
    [DataContract]
    [KnownType(typeof(SelectorNode))]
    [KnownType(typeof(SequenceNode))]
    [KnownType(typeof(ConditionNode))]
    [KnownType(typeof(InverterNode))]
    [KnownType(typeof(LoopNode))]
    [KnownType(typeof(NavigateNode))]
    public class BehaviorTree
    {
        public BehaviorStatuses Status { get; private set; }
        public Dictionary<string, object> VariablesByName { get; protected set; } = new Dictionary<string, object>();

        [DataMember]
        public INode RootNode { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext c)
        {
            VariablesByName = new Dictionary<string, object>();
        }

        public void Tick()
        {
            if (!Status.IsComplete())
            {
                RootNode.Tick(VariablesByName);
                Status = RootNode.Status;
            }
            else
            {
                Reset();
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
                var serializer = new DataContractSerializer(typeof(BehaviorTree));
                serializer.WriteObject(writer, this);
            }
        }

        public static BehaviorTree Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new DataContractSerializer(typeof(BehaviorTree));
                return serializer.ReadObject(reader, true) as BehaviorTree;
            }
        }

        public static void SaveTestEnemyBehavior()
        {
            // We need to create the behavior where the enemy will patrol a set of points
            var behavior = new BehaviorTree
            {
                RootNode = new SequenceNode(
                    new List<INode>()
                    {
                        new NavigateNode(new Vector3(5.0f, 5.0f, -1.0f), 0.1f),
                        new NavigateNode(new Vector3(-1.0f, -1.0f, -1.0f), 0.1f)
                    }
                )
            };

            behavior.Save(FilePathHelper.ENEMY_BEHAVIOR_PATH);
        }
    }
}
