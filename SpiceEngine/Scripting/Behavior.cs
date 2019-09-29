using SpiceEngine.Scripting.Nodes;
using SpiceEngine.Scripting.StimResponse;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Physics.Collisions;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.Scripting.Properties;
using SpiceEngineCore.Scripting.StimResponse;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;

namespace SpiceEngine.Scripting
{
    public class Behavior : IBehavior
    {
        private Stack<Node> _rootStack = new Stack<Node>();
        private List<Response> _responses = new List<Response>();

        public BehaviorContext Context { get; private set; } = new BehaviorContext();

        public void SetActor(IActor actor) => Context.Actor = actor;
        public void SetCamera(ICamera camera) => Context.Camera = camera;
        public void SetEntityProvider(IEntityProvider entityProvider) => Context.SetEntityProvider(entityProvider);
        public void SetCollisionProvider(ICollisionProvider collisionProvider) => Context.SetCollisionProvider(collisionProvider);
        public void SetInputProvider(IInputProvider inputProvider) => Context.SetInputProvider(inputProvider);
        public void SetStimulusProvider(IStimulusProvider stimulusProvider) => Context.SetStimulusProvider(stimulusProvider);
        public void SetProperty(string name, object value) => Context.SetProperty(name, value);

        public void PushRootNode(Node node)
        {
            if (node is ScriptNode scriptNode)
            {
                scriptNode.Compiled += (s, args) => _rootStack.Push(args.Node);
            }
            else
            {
                _rootStack.Push(node);
            }
        }

        public void AddResponse(Response response) => _responses.Add(response);

        public BehaviorStatus Tick()
        {
            foreach (var response in _responses)
            {
                response.Tick(Context);
            }

            var root = _rootStack.Peek();
            var rootStatus = root.Tick(Context);

            if (rootStatus.IsComplete())
            {
                _rootStack.Pop();
            }

            return rootStatus;
        }
    }
}
