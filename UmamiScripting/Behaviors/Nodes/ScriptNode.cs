using System;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore.Behaviors.Nodes.Composites;
using UmamiScriptingCore.Behaviors.Nodes.Decorators;
using UmamiScriptingCore.Scripts;
using ValueType = SpiceEngineCore.Serialization.ValueType;

namespace UmamiScriptingCore.Behaviors.Nodes
{
    public class ScriptNode : Node
    {
        private IScript _script;
        private List<ValueType> _arguments = new List<ValueType>();
        private List<Node> _children = new List<Node>();

        public event EventHandler<NodeEventArgs> Compiled;

        public ScriptNode(IScript script)
        {
            _script = script;
            _script.Compiled += (s, args) =>
            {
                var compiledNode = ToCompiledNode();
                Compiled?.Invoke(this, new NodeEventArgs(compiledNode));
            };
        }

        public ScriptNode(IScript script, IEnumerable<ValueType> arguments, IEnumerable<Node> children)
        {
            _script = script;
            _script.Compiled += (s, args) =>
            {
                var compiledNode = ToCompiledNode();
                Compiled?.Invoke(this, new NodeEventArgs(compiledNode));
            };

            _arguments.AddRange(arguments);
            _children.AddRange(children);
        }

        // TODO - Because this is event-based at the moment, Tick() should just be a no-op since it can happen if the node hasn't been replaced yet
        //public override BehaviorStatus Tick(BehaviorContext context) => throw new InvalidOperationException("");
        public override BehaviorStatus Tick(BehaviorContext context) => BehaviorStatus.Failure;

        public override void Reset() { } // NO-OP

        private Node ToCompiledNode()
        {
            if (_script.HasErrors || !_script.IsCompiled)
            {
                // TODO - Handle errors by notifying user
                throw new Exception("Script node compilation failed");
            }
            else
            {
                var type = _script.ExportedType;

                if (type.IsSubclassOf(typeof(CompositeNode)))
                {
                    if (_arguments.Count > 0)
                    {
                        return (Node)Activator.CreateInstance(type, _children, GetConvertedArguments().ToArray());
                    }
                    else
                    {
                        return (Node)Activator.CreateInstance(type, _children);
                    }
                }
                else if (type.IsSubclassOf(typeof(DecoratorNode)))
                {
                    if (_arguments.Count > 0)
                    {
                        return (Node)Activator.CreateInstance(type, _children.First(), GetConvertedArguments().ToArray());
                    }
                    else
                    {
                        return (Node)Activator.CreateInstance(type, _children.First());
                    }
                }
                else if (type.IsSubclassOf(typeof(Node)))
                {
                    if (_arguments.Count > 0)
                    {
                        return (Node)Activator.CreateInstance(type, GetConvertedArguments().ToArray());
                    }
                    else
                    {
                        return (Node)Activator.CreateInstance(type);
                    }
                }
                else
                {
                    // TODO - Notify user that the type MUST derive from Node
                }
            }

            return null;
        }

        private IEnumerable<object> GetConvertedArguments()
        {
            foreach (var arg in _arguments)
            {
                yield return arg.Cast();

                /*if (arg is double)
                {
                    yield return Convert.ChangeType(arg, typeof(float));
                }
                else if (arg is short || arg is long)
                {
                    yield return Convert.ChangeType(arg, typeof(int));
                }
                else
                {
                    yield return arg;
                }*/
            }
        }
    }
}
