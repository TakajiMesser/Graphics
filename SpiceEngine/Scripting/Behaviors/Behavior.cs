using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SpiceEngine.Entities;
using SpiceEngine.Scripting.StimResponse;

namespace SpiceEngine.Scripting.Behaviors
{
    public abstract class Behavior
    {
        public Stack<Node> RootStack { get; private set; } = new Stack<Node>();
        public BehaviorContext Context { get; private set; } = new BehaviorContext();
        public List<Response> Responses { get; private set; } = new List<Response>();

        public Behavior()
        {
            SetRootNodes();
            SetResponses();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext c) => Context = new BehaviorContext();

        protected abstract void SetRootNodes();
        protected abstract void SetResponses();

        public virtual BehaviorStatus Tick()
        {
            foreach (var response in Responses)
            {
                response.Tick(Context);
            }

            var root = RootStack.Peek();
            var rootStatus = root.Tick(Context);

            if (rootStatus.IsComplete())
            {
                RootStack.Pop();
            }

            return rootStatus;
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
