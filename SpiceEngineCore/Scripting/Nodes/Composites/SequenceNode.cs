using SpiceEngineCore.Utilities;
using System.Collections.Generic;

namespace SpiceEngineCore.Scripting.Nodes.Composites
{
    /// <summary>
    /// Attempts to tick each child forward. Returns success once all children have finished. Returns failure if any child fails.
    /// </summary>
    public class SequenceNode : CompositeNode
    {
        private int _currentIndex = 0;

        public SequenceNode(params Node[] children) : base(children) { }
        public SequenceNode(IEnumerable<Node> children) : base(children) { }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            switch (Children[_currentIndex].Tick(context))
            {
                case BehaviorStatus.Success:
                    _currentIndex++;
                    if (_currentIndex >= Children.Count)
                    {
                        OnCompleted(new BehaviorCompletedEventArgs(BehaviorStatus.Success));
                        return BehaviorStatus.Success;
                    }
                    break;
                case BehaviorStatus.Failure:
                    OnCompleted(new BehaviorCompletedEventArgs(BehaviorStatus.Failure));
                    return BehaviorStatus.Failure;
            }

            return BehaviorStatus.Running;
        }

        public override void Reset()
        {
            _currentIndex = 0;
            base.Reset();
        }
    }
}
