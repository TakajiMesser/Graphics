using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Physics;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;
using UmamiScriptingCore.Behaviors.Nodes;
using UmamiScriptingCore.Behaviors.StimResponse;

namespace UmamiScriptingCore.Behaviors
{
    public class Behavior : IBehavior
    {
        private Stack<Node> _rootStack = new Stack<Node>();
        private List<IResponse> _responses = new List<IResponse>();

        public BehaviorContext Context { get; private set; } = new BehaviorContext();

        public void SetEntity(IEntity entity) => Context.Entity = entity;
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

        public void AddResponse(IResponse response) => _responses.Add(response);

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
