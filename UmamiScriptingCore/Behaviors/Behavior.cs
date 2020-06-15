using SpiceEngineCore.Components;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Game;
using System.Collections.Generic;
using System.Data;
using UmamiScriptingCore.Behaviors.Nodes;
using UmamiScriptingCore.StimResponse;
using UmamiScriptingCore.Utilities;

namespace UmamiScriptingCore.Behaviors
{
    public class Behavior : Component, IBehavior
    {
        private Stack<Node> _rootStack = new Stack<Node>();
        private List<IResponse> _responses = new List<IResponse>();
        private List<IStimulus> _stimuli = new List<IStimulus>();
        private PropertyCollection _properties = new PropertyCollection();

        public Behavior(int entityID) : base(entityID) { }

        public BehaviorContext Context { get; private set; }

        public void SetSystemProvider(ISystemProvider systemProvider) => Context = new BehaviorContext(this, systemProvider);
        public void SetCamera(ICamera camera) => Context.Camera = camera;
        public void SetProperty(string name, object value) => Context.SetProperty(name, value);

        /*public void SetCollisionProvider(ICollisionProvider collisionProvider) => Context.SetCollisionProvider(collisionProvider);
        public void SetInputProvider(IInputProvider inputProvider) => Context.SetInputProvider(inputProvider);
        public void SetStimulusProvider(IStimulusProvider stimulusProvider) => Context.SetStimulusProvider(stimulusProvider);
        public void SetSelectionTracker(ISelectionTracker selectionTracker) => Context.SetSelectionTracker(selectionTracker);
        public void SetUIProvider(IUIProvider uiProvider) => Context.SetUIProvider(uiProvider);*/

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

            if (_rootStack.Count > 0)
            {
                var root = _rootStack.Peek();
                var rootStatus = root.Tick(Context);

                if (rootStatus.IsComplete())
                {
                    _rootStack.Pop();
                }

                return rootStatus;
            }

            return BehaviorStatus.Dormant;
        }
    }
}
