using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.Behaviors.Composites
{
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
                        _currentIndex = 0;
                        OnCompleted(new BehaviorCompletedEventArgs(BehaviorStatus.Success));
                        return BehaviorStatus.Success;
                    case BehaviorStatus.Failure:
                        _currentIndex++;
                        if (_currentIndex >= Children.Count)
                        {
                            _currentIndex = 0;
                            OnCompleted(new BehaviorCompletedEventArgs(BehaviorStatus.Failure));
                            return BehaviorStatus.Failure;
                        }
                        break;
                }
            }

            return BehaviorStatus.Running;
        }
    }
}
