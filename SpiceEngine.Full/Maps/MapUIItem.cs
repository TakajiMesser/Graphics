using SpiceEngine.GLFWBindings.Inputs;
using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.UserInterfaces;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using StarchUICore;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Styling;
using StarchUICore.Attributes.Units;
using StarchUICore.Groups;
using StarchUICore.Views;
using StarchUICore.Views.Controls.Buttons;
using SweetGraphicsCore.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;
using UmamiScriptingCore.Behaviors.Nodes.Decorators;
using UmamiScriptingCore.Props;
using UmamiScriptingCore.Scripts;
using UmamiScriptingCore.StimResponse;
using Color4 = SpiceEngineCore.Geometry.Color4;
using InputTypes = UmamiScriptingCore.Behaviors.Nodes.Decorators.InputTypes;

namespace SpiceEngine.Maps
{
    public enum UITypes
    {
        View,
        RowGroup,
        ColumnGroup,
        LayerGroup,
        Button,
        Label,
        Panel
    }

    public class MapUIItem : MapEntity<UIItem>, IMapUIItem, ITexturePather, IBehaviorBuilder, IElementBuilder
    {
        public string Name { get; set; }
        public List<string> ChildElementNames { get; set; } = new List<string>();

        public string X { get; set; }
        public string Y { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        public string MinimumX { get; set; }
        public string MinimumY { get; set; }
        public string MinimumWidth { get; set; }
        public string MinimumHeight { get; set; }

        public string MaximumX { get; set; }
        public string MaximumY { get; set; }
        public string MaximumWidth { get; set; }
        public string MaximumHeight { get; set; }

        public AnchorTypes HorizontalAnchorSelfType { get; set; }
        public AnchorTypes HorizontalAnchorRelativeType { get; set; }
        public string RelativeHorizontalAnchorElementName { get; set; }
        public bool DoesHorizontalAnchorRespectChanges { get; set; }

        public AnchorTypes VerticalAnchorSelfType { get; set; }
        public AnchorTypes VerticalAnchorRelativeType { get; set; }
        public string RelativeVerticalAnchorElementName { get; set; }
        public bool DoesVerticalAnchorRespectChanges { get; set; }

        public string RelativeHorizontalDockElementName { get; set; }
        public bool DoesHorizontalDockRespectChanges { get; set; }

        public string RelativeVerticalDockElementName { get; set; }
        public bool DoesVerticalDockRespectChanges { get; set; }

        public string PaddingLeft { get; set; }
        public string PaddingTop { get; set; }
        public string PaddingRight { get; set; }
        public string PaddingBottom { get; set; }

        public Color4 Color { get; set; }
        public float Alpha { get; set; } = 1.0f;

        public float BorderThickness { get; set; }
        public Color4 BorderColor { get; set; }
        public float CornerXRadius { get; set; }
        public float CornerYRadius { get; set; }

        public string FontFilePath { get; set; }
        public int FontSize { get; set; } = 12;

        public UITypes UIType { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();

        public Script PushScript { get; set; }

        public override IEntity ToEntity() => new UIItem()
        {
            Name = Name,
            //Position = new Vector3(X, Y, 0.0f)
        };

        public void UpdateFrom(IUIItem uiItem)
        {

        }

        private IElement _component;
        private object _componentLock = new object();
        private int _entityID;

        IElement IComponentBuilder<IElement>.ToComponent(int entityID)
        {
            lock (_componentLock)
            {
                if (_component == null)
                {
                    _component = CreateComponent(entityID);
                    _entityID = entityID;
                }

                return _component;
            }
        }

        public IRenderable ToRenderable()
        {
            lock (_componentLock)
            {
                if (_component == null)
                {
                    _component = CreateComponent(_entityID);
                }

                return _component;
            }
        }

        public IEnumerable<IScript> GetScripts() => PushScript.Yield();///*Behavior != null ? Behavior.GetScripts() : */Enumerable.Empty<IScript>();
        public IEnumerable<IStimulus> GetStimuli() => Enumerable.Empty<IStimulus>();
        public IEnumerable<IProperty> GetProperties() => Enumerable.Empty<IProperty>();

        IBehavior IComponentBuilder<IBehavior>.ToComponent(int entityID)
        {
            if (UIType == UITypes.Button && PushScript != null)
            {
                var behavior = new Behavior(entityID);

                // TODO - Check to see if this button was both pressed and released
                var rootNode = new RepeaterNode(new InputConditionNode(
                    new SelectionConditionNode(
                        new ScriptNode(PushScript)
                    ),
                    new MouseButtonCondition(MouseButtons.Left, InputTypes.Released)
                ));

                /*var pushNode = new ScriptNode(PushScript);
                var rootNode = new InputConditionNode(pushNode, InputTypes.Released, new Input(OpenTK.Input.MouseButton.Left));*/
                behavior.PushRootNode(rootNode);

                return behavior;
            }

            return null;
        }

        private IElement CreateComponent(int entityID)
        {
            if (TexturesPaths.Any()) throw new NotImplementedException();

            switch (UIType)
            {
                case UITypes.View:
                    var view = new View(entityID);
                    ApplyValues(view);
                    return view;
                case UITypes.RowGroup:
                    var rowGroup = new RowGroup(entityID);
                    ApplyValues(rowGroup);
                    return rowGroup;
                case UITypes.ColumnGroup:
                    var columnGroup = new ColumnGroup(entityID);
                    ApplyValues(columnGroup);
                    return columnGroup;
                case UITypes.LayerGroup:
                    var layerGroup = new LayerGroup(entityID);
                    ApplyValues(layerGroup);
                    return layerGroup;
                case UITypes.Button:
                    var button = new Button(entityID);
                    ApplyValues(button);
                    return button;
                case UITypes.Label:
                    var textView = new Label(entityID);
                    ApplyValues(textView);
                    return textView;
            }

            throw new NotImplementedException();
        }

        private void ApplyValues(Element element)
        {
            element.Name = Name;
            element.Position = ParsePosition();
            element.Size = ParseSize();
            element.HorizontalAnchor = new Anchor(HorizontalAnchorSelfType, HorizontalAnchorRelativeType, DoesHorizontalAnchorRespectChanges);
            element.VerticalAnchor = new Anchor(VerticalAnchorSelfType, VerticalAnchorRelativeType, DoesVerticalAnchorRespectChanges);
            element.HorizontalDock = new Dock(DoesHorizontalDockRespectChanges);
            element.VerticalDock = new Dock(DoesVerticalDockRespectChanges);
            element.Alpha = Alpha;
            element.Padding = ParsePadding();
            element.Border = new Border(BorderThickness, BorderColor, CornerXRadius, CornerYRadius);

            if (element is View view)
            {
                view.Background = new StarchUICore.Layers.Layer
                {
                    Color = Color
                };
            }

            if (element is Button button)
            {
                
            }

            if (element is Group group)
            {
                /*foreach (var child in _children)
                {
                    group.AddChild(child);
                }*/
            }

            if (element is Label textView)
            {
                textView.Text = "Blarg";
                textView.Color = Color4.Black;
                //textView.Font = ;
            }
        }

        private Position ParsePosition()
        {
            var x = !string.IsNullOrEmpty(X) ? ParseUnits(X) : Unit.Auto();
            var y = !string.IsNullOrEmpty(Y) ? ParseUnits(Y) : Unit.Auto();
            var minimumX = !string.IsNullOrEmpty(MinimumX) ? ParseUnits(MinimumX) : Unit.Auto();
            var minimumY = !string.IsNullOrEmpty(MinimumY) ? ParseUnits(MinimumY) : Unit.Auto();
            var maximumX = !string.IsNullOrEmpty(MaximumX) ? ParseUnits(MaximumX) : Unit.Auto();
            var maximumY = !string.IsNullOrEmpty(MaximumY) ? ParseUnits(MaximumY) : Unit.Auto();

            return new Position(x, y, minimumX, minimumY, maximumX, maximumY);
        }

        private Size ParseSize()
        {
            var width = !string.IsNullOrEmpty(Width) ? ParseUnits(Width) : Unit.Auto();
            var height = !string.IsNullOrEmpty(Height) ? ParseUnits(Height) : Unit.Auto();
            var minimumWidth = !string.IsNullOrEmpty(MinimumWidth) ? ParseUnits(MinimumWidth) : Unit.Auto();
            var minimumHeight = !string.IsNullOrEmpty(MinimumHeight) ? ParseUnits(MinimumHeight) : Unit.Auto();
            var maximumWidth = !string.IsNullOrEmpty(MaximumWidth) ? ParseUnits(MaximumWidth) : Unit.Auto();
            var maximumHeight = !string.IsNullOrEmpty(MaximumHeight) ? ParseUnits(MaximumHeight) : Unit.Auto();

            return new Size(width, height, minimumWidth, minimumHeight, maximumWidth, maximumHeight);
        }

        /*private Anchor ParseAnchor()
        {
            
        }

        private Dock ParseDock()
        {

        }*/

        private Padding ParsePadding()
        {
            var left = !string.IsNullOrEmpty(PaddingLeft) ? ParseUnits(PaddingLeft) : Unit.Auto();
            var top = !string.IsNullOrEmpty(PaddingTop) ? ParseUnits(PaddingTop) : Unit.Auto();
            var right = !string.IsNullOrEmpty(PaddingRight) ? ParseUnits(PaddingRight) : Unit.Auto();
            var bottom = !string.IsNullOrEmpty(PaddingBottom) ? ParseUnits(PaddingBottom) : Unit.Auto();

            return new Padding(left, top, right, bottom);
        }

        private IUnits ParseUnits(string value)
        {
            if (value.ToLower() == "auto")
            {
                return Unit.Auto();
            }
            else if (value.EndsWith("%"))
            {
                if (float.TryParse(value.Substring(0, value.Length - 1), out float result))
                {
                    return Unit.Percents(result);
                }
                else
                {
                    throw new ArgumentException("Failed to correctly parse Percent Units");
                }
            }
            else
            {
                if (int.TryParse(value, out int result))
                {
                    return Unit.Pixels(result);
                }
                else
                {
                    throw new ArgumentException("Failed to correctly parse Pixel Units");
                }
            }
        }
    }
}
