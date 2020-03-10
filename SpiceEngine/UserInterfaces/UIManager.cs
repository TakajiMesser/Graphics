using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Outputs;
using SpiceEngineCore.UserInterfaces;
using StarchUICore;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Groups;
using System.Collections.Generic;

namespace SpiceEngine.UserInterfaces
{
    // Stores all UIControls and determines the order that they should be drawn in
    public class UIManager : ComponentLoader<IUIElement, IUIElementBuilder>, IUIProvider
    {
        private Resolution _resolution;
        private int _rootID;
        private SetDictionary<int, int> _childIDSetByID = new SetDictionary<int, int>();

        public UIManager(IEntityProvider entityProvider, Resolution resolution)
        {
            SetEntityProvider(entityProvider);
            _resolution = resolution;
        }

        private IElement GetRoot() => _rootID > 0 ? _componentByID[_rootID] as IElement : null;

        private Dictionary<int, string> _horizontalAnchorNamesByID = new Dictionary<int, string>();
        private Dictionary<int, string> _verticalAnchorNamesByID = new Dictionary<int, string>();
        private Dictionary<int, string> _horizontalDockNamesByID = new Dictionary<int, string>();
        private Dictionary<int, string> _verticalDockNamesByID = new Dictionary<int, string>();
        private SetDictionary<int, string> _childNamesByID = new SetDictionary<int, string>();

        public override void LoadBuilderSync(int entityID, IUIElementBuilder builder)
        {
            base.LoadBuilderSync(entityID, builder);

            _horizontalAnchorNamesByID.Add(entityID, builder.RelativeHorizontalAnchorElementName);
            _verticalAnchorNamesByID.Add(entityID, builder.RelativeVerticalAnchorElementName);
            _horizontalDockNamesByID.Add(entityID, builder.RelativeHorizontalDockElementName);
            _verticalDockNamesByID.Add(entityID, builder.RelativeVerticalDockElementName);
            _childNamesByID.AddRange(entityID, builder.ChildElementNames);
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();

            // Now that all elements have at least been constructed, we can freely assign parents and children
            foreach (var componentAndID in _componentsAndIDs)
            {
                var element = componentAndID.Item1 as IElement;

                if (element is IGroup group)
                {
                    var childNames = _childNamesByID.GetValues(componentAndID.Item2);
                    
                    foreach (var childName in childNames)
                    {
                        if (!string.IsNullOrEmpty(childName))
                        {
                            var childID = _entityProvider.GetEntity(childName).ID;
                            
                            var childElement = GetElement(childID);
                            group.AddChild(childElement);

                            _childIDSetByID.Add(componentAndID.Item2, childID);
                        }
                    }

                    /*var parentEntity = _entityProvider.GetEntity(componentAndID.Item2) as IParentEntity;

                    foreach (var childID in parentEntity.ChildIDs)
                    {
                        _childIDSetByID.Add(componentAndID.Item2, childID);
                    }

                    parentEntity.ChildrenAdded += (s, args) => _childIDSetByID.AddRange(componentAndID.Item2, args.IDs);
                    parentEntity.ChildrenRemoved += (s, args) =>
                    {
                        foreach (var id in args.IDs)
                        {
                            _childIDSetByID.Remove(componentAndID.Item2, id);
                        }
                    };*/
                }

                var horizontalAnchorName = _horizontalAnchorNamesByID[componentAndID.Item2];
                element.HorizontalAnchor = element.HorizontalAnchor.Attached(GetElementByName(horizontalAnchorName));

                var verticalAnchorName = _verticalAnchorNamesByID[componentAndID.Item2];
                element.VerticalAnchor = element.VerticalAnchor.Attached(GetElementByName(verticalAnchorName));

                var horizontalDockName = _horizontalDockNamesByID[componentAndID.Item2];
                element.HorizontalDock = element.HorizontalDock.Attached(GetElementByName(horizontalDockName));

                var verticalDockName = _verticalDockNamesByID[componentAndID.Item2];
                element.VerticalDock = element.VerticalDock.Attached(GetElementByName(verticalDockName));
            }

            // TODO - Iterating twice here is pretty shit
            foreach (var componentAndID in _componentsAndIDs)
            {
                var element = componentAndID.Item1 as IElement;

                // TODO - Do we want to allow more than one root UI element?
                if (element.Parent == null)
                {
                    _rootID = componentAndID.Item2;
                }
            }

            // TODO - Also determine the root ID here...
            _horizontalAnchorNamesByID.Clear();
            _verticalAnchorNamesByID.Clear();
            _horizontalDockNamesByID.Clear();
            _verticalDockNamesByID.Clear();
            _childNamesByID.Clear();
        }

        private IElement GetElementByName(string entityName)
        {
            if (!string.IsNullOrEmpty(entityName))
            {
                var id = _entityProvider.GetEntity(entityName).ID;
                return GetElement(id);
            }
            else
            {
                return null;
            }
        }

        /*protected override void LoadComponent(int entityID, IUIElement component)
        {
            if (component is IElement element)
            {
                if (element is IGroup group)
                {
                    _childNamesByID[entityID];
                }
                element.Parent;
            }
        }

        public void AddElement(int entityID, IElement element)
        {
            if (element.Parent == null)
            {
                _rootID = entityID;
            }

            _elementByID.Add(entityID, element);

            if (element is IGroup group)
            {
                var parentEntity = _entityProvider.GetEntity(entityID) as IParentEntity;

                foreach (var childID in parentEntity.ChildIDs)
                {
                    _childIDSetByID.Add(entityID, childID);
                }

                parentEntity.ChildrenAdded += (s, args) => _childIDSetByID.AddRange(entityID, args.IDs);
                parentEntity.ChildrenRemoved += (s, args) =>
                {
                    foreach (var id in args.IDs)
                    {
                        _childIDSetByID.Remove(entityID, id);
                    }
                };
            }

            /*item.PositionChanged += (s, args) =>
            {
                var entity = _entityProvider.GetEntity(entityID);
                var position = entity.Position;

                entity.Position = new Vector3(args.NewPosition.X, args.NewPosition.Y, position.Z);
            };*
        }*/

        public IElement GetElement(int entityID) => _componentByID[entityID] as IElement;

        public void Clear()
        {
            _rootID = 0;
            //_elementByID.Clear();
            _childIDSetByID.Clear();
        }

        public void Load() => GetRoot()?.Load();

        private int _tickCounter = 0;

        protected override void Update()
        {
            var root = GetRoot();

            // TODO - This is test code to move the root view every 150 ticks
            _tickCounter++;

            if (_tickCounter == 150)
            {
                _tickCounter = 0;

                if (root != null)
                {
                    var position = root.Position;
                    //root.Position = new Position(Unit.Pixels(position.X.Constrain( + 100, position.Y + 100);
                }
                
            }

            root?.Layout(new LayoutInfo(_resolution.Width, _resolution.Height, _resolution.Width, _resolution.Height, 0, 0, 0, 0));
            //root?.Measure(new MeasuredSize(_resolution.Width, _resolution.Height));
            root?.Update(TickRate);
        }

        public IEnumerable<int> GetDrawOrder()
        {
            var root = GetRoot();

            foreach (var id in GetDrawOrder(_rootID, root))
            {
                yield return id;
            }
        }

        private IEnumerable<int> GetDrawOrder(int id, IElement element)
        {
            if (element != null && element.IsVisible)
            {
                yield return id;

                if (_childIDSetByID.TryGetValues(id, out IEnumerable<int> childIDs))
                {
                    foreach (var childID in childIDs)
                    {
                        var childElement = GetElement(childID);

                        foreach (var grandChildID in GetDrawOrder(childID, childElement))
                        {
                            yield return grandChildID;
                        }
                    }
                }
            }
        }

        public void Draw() => GetRoot()?.Draw();

        /*private void DrawRecursively(IUIItem item)
        {
            item.Draw();

            if (item is IGroup group)
            {
                foreach (var child in group.GetChildren())
                {
                    DrawRecursively(child);
                }
            }
            else if (item is IView view)
            {
                view.Draw();
            }
        }*/
    }
}
