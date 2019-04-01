using SpiceEngine.Scripting.StimResponse;
using System.Collections.Generic;

namespace SpiceEngine.Scripting
{
    public class Behavior
    {
        private Stack<Node> _rootStack = new Stack<Node>();
        private List<Response> _responses = new List<Response>();

        public BehaviorContext Context { get; private set; } = new BehaviorContext();

        public void PushRootNode(Node node)
        {
            _rootStack.Push(node);
        }

        public void AddResponse(Response response)
        {
            _responses.Add(response);
        }

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
