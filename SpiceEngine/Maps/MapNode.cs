﻿using System;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;
using UmamiScriptingCore.Behaviors.Nodes.Composites;
using UmamiScriptingCore.Behaviors.Nodes.Decorators;
using UmamiScriptingCore.Behaviors.Nodes.Leaves;
using UmamiScriptingCore.Scripts;
using ValueType = SpiceEngineCore.Serialization.ValueType;

namespace SpiceEngine.Maps
{
    public class MapNode
    {
        public enum NodeTypes
        {
            Parallel,
            Selector,
            Sequence,
            InlineCondition,
            Repeater,
            InlineLeaf,
            Leaf,
            Script,
            Camera
        };

        public MapNode() { }
        public MapNode(params object[] args) => Arguments.AddRange(args.Select(a => ValueType.Create(a)));

        public List<ValueType> Arguments{ get; set; } = new List<ValueType>();
        public NodeTypes NodeType { get; set; }
        public List<MapNode> Children { get; set; } = new List<MapNode>();

        public Predicate<BehaviorContext> InlineCondition { get; set; }
        public Action<BehaviorContext> InlineAction { get; set; }

        public Type LeafType { get; set; }

        // For more complex custom nodes, we must parse and analyze the node's C# code
        public Script Script { get; set; }

        public IEnumerable<Script> GetScripts()
        {
            if (NodeType == NodeTypes.Script)
            {
                yield return Script;
            }

            foreach (var child in Children)
            {
                foreach (var script in child.GetScripts())
                {
                    yield return script;
                }
            }
        }

        public const float CAMERA_MOVE_SPEED = 0.02f;
        public const float CAMERA_TURN_SPEED = 0.001f;
        public const float CAMERA_ZOOM_SPEED = 1.0f;

        public Node ToNode()
        {
            switch (NodeType)
            {
                case NodeTypes.Parallel:
                    return new ParallelNode(Children.Select(c => c.ToNode()));
                case NodeTypes.Selector:
                    return new SelectorNode(Children.Select(c => c.ToNode()));
                case NodeTypes.Sequence:
                    return new SequenceNode(Children.Select(c => c.ToNode()));
                case NodeTypes.InlineCondition:
                    return new InlineConditionNode(InlineCondition, Children.First().ToNode());
                case NodeTypes.Repeater:
                    return new RepeaterNode(Children.First().ToNode());
                case NodeTypes.InlineLeaf:
                    return new InlineLeafNode(InlineAction);
                case NodeTypes.Script:
                    // We now need to DELAY this...
                    return new ScriptNode(Script, Arguments, Children.Select(c => c.ToNode()));
                    //return ParseContent(scriptCompiler);
                case NodeTypes.Leaf:
                    return (Node)Activator.CreateInstance(LeafType, GetConvertedArguments().ToArray());
                case NodeTypes.Camera:
                    return new CameraNode(CAMERA_MOVE_SPEED, CAMERA_TURN_SPEED, CAMERA_ZOOM_SPEED);
            }

            // TODO - Perform a better default case
            return null;
        }

        private IEnumerable<object> GetConvertedArguments()
        {
            foreach (var arg in Arguments)
            {
                yield return arg.Cast();
            }
        }

        /*private Node ParseContent(IScriptCompiler scriptCompiler)
        {
            // TODO - Perform script compilation in an earlier step
            scriptCompiler.AddScript(Script);
            scriptCompiler.CompileScripts();
            scriptCompiler.ClearScripts();

            if (Script.HasErrors)
            {
                // TODO - Handle errors by notifying user
            }
            else
            {
                var type = Script.ExportedType;

                if (type.IsSubclassOf(typeof(CompositeNode)))
                {
                    if (Arguments.Count > 0)
                    {
                        return (Node)Activator.CreateInstance(type, Children.Select(c => c.ToNode()), GetConvertedArguments().ToArray());
                    }
                    else
                    {
                        return (Node)Activator.CreateInstance(type, Children.Select(c => c.ToNode()));
                    }
                }
                else if (type.IsSubclassOf(typeof(DecoratorNode)))
                {
                    if (Arguments.Count > 0)
                    {
                        return (Node)Activator.CreateInstance(type, Children.First().ToNode(), GetConvertedArguments().ToArray());
                    }
                    else
                    {
                        return (Node)Activator.CreateInstance(type, Children.First().ToNode());
                    }
                }
                else if (type.IsSubclassOf(typeof(Node)))
                {
                    if (Arguments.Count > 0)
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
        }*/
    }
}
