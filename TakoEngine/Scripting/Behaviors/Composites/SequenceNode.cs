using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.Behaviors.Composites
{
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
                        _currentIndex = 0;
                        OnCompleted(new BehaviorCompletedEventArgs(BehaviorStatus.Success));
                        return BehaviorStatus.Success;
                    }
                    break;
                case BehaviorStatus.Failure:
                    _currentIndex = 0;
                    OnCompleted(new BehaviorCompletedEventArgs(BehaviorStatus.Failure));
                    return BehaviorStatus.Failure;
            }

            return BehaviorStatus.Running;
        }
    }
}
