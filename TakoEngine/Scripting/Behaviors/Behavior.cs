using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TakoEngine.Scripting.Behaviors
{
    public abstract class Behavior
    {
        public Stack<Node> RootStack { get; private set; } = new Stack<Node>();
        public BehaviorContext Context { get; private set; } = new BehaviorContext();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext c)
        {
            Context = new BehaviorContext();
        }

        public virtual BehaviorStatus Tick()
        {
            var root = RootStack.Peek();
            return root.Tick(Context);
        }

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static Behavior Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as Behavior;
            }
        }
    }
}
