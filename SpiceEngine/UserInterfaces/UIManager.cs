using SpiceEngineCore.Entities;
using SpiceEngineCore.Game;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Outputs;
using StarchUICore;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Groups;
using System.Collections.Generic;

namespace SpiceEngine.UserInterfaces
{
    // Stores all UIControls and determines the order that they should be drawn in
    public class UIManager : UpdateManager, IUIProvider
    {
        private IEntityProvider _entityProvider;
        private Dictionary<int, IElement> _elementByID = new Dictionary<int, IElement>();

        private int _rootID;
        private SetDictionary<int, int> _childIDSetByID = new SetDictionary<int, int>();

        public UIManager(IEntityProvider entityProvider, Resolution resolution)
        {
            _entityProvider = entityProvider;
            Resolution = resolution;
        }

        public Resolution Resolution { get; }

        private IElement GetRoot() => _rootID > 0 ? _elementByID[_rootID] : null;

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
            };*/
        }

        public IElement GetElement(int entityID) => _elementByID[entityID];

        public void Clear()
        {
            _rootID = 0;
            _elementByID.Clear();
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

            root?.Measure(new MeasuredSize(Resolution.Width, Resolution.Height));
            root?.Update();
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
                        var childElement = _elementByID[childID];

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
