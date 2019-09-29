using SpiceEngineCore.Utilities;
using System.Collections.Generic;

namespace SpiceEngine.Scripting.Nodes.Composites
{
    /// <summary>
    /// Attempts to tick each child forward. Returns success upon the first successful tick. Returns failure if no children tick successfully.
    /// </summary>
    public class SelectorNode : CompositeNode
    {
        private int _currentIndex = 0;

        public SelectorNode(params Node[] children) : base(children) { }
        public SelectorNode(IEnumerable<Node> children) : base(children) { }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            for (var i = _currentIndex; i < Children.Count; i++)
            {
                switch (Children[_currentIndex].Tick(context))
                {
                    case BehaviorStatus.Success:
                        OnCompleted(new BehaviorCompletedEventArgs(BehaviorStatus.Success));
                        return BehaviorStatus.Success;
                    case BehaviorStatus.Failure:
                        _currentIndex++;
                        if (_currentIndex >= Children.Count)
                        {
                            OnCompleted(new BehaviorCompletedEventArgs(BehaviorStatus.Failure));
                            return BehaviorStatus.Failure;
                        }
                        break;
                }
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
