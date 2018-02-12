﻿using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Inputs;
using Graphics.Physics.Collision;
using Graphics.Physics.Raycasting;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using Graphics.Scripting.BehaviorTrees.Leaves;
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
    [KnownType(typeof(ParallelNode))]
    [KnownType(typeof(RepeaterNode))]
    [KnownType(typeof(InverterNode))]
    [KnownType(typeof(LoopNode))]
    [KnownType(typeof(ConditionNode))]
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
