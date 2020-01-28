using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Outputs;
using StarchUICore;
using StarchUICore.Attributes;
using StarchUICore.Groups;
using StarchUICore.Views;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.UserInterfaces
{
    // Stores all UIControls and determines the order that they should be drawn in
    public class UIManager : UpdateManager, IUIProvider
    {
        private IEntityProvider _entityProvider;
        private Dictionary<int, IUIItem> _itemByID = new Dictionary<int, IUIItem>();

        private List<int> _rootIDs = new List<int>();
        private SetDictionary<int, int> _childIDSetByID = new SetDictionary<int, int>();

        public UIManager(IEntityProvider entityProvider, Resolution resolution)
        {
            _entityProvider = entityProvider;
            Resolution = resolution;
        }

        public Resolution Resolution { get; }

        private IEnumerable<IUIItem> GetRootItems()
        {
            foreach (var id in _rootIDs)
            {
                yield return _itemByID[id];
            }
        }

        public void AddItem(int entityID, IUIItem item)
        {
            if (item.Parent == null)
            {
                _rootIDs.Add(entityID);
            }

            _itemByID.Add(entityID, item);

            if (item is IGroup group)
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

            item.PositionChanged += (s, args) =>
            {
                var entity = _entityProvider.GetEntity(entityID);
                var position = entity.Position;

                entity.Position = new Vector3(args.NewPosition.X, args.NewPosition.Y, position.Z);
            };
        }

        public IUIItem GetItem(int entityID) => _itemByID[entityID];

        public void Clear()
        {
            _itemByID.Clear();
            _rootIDs.Clear();
            _childIDSetByID.Clear();
        }

        public void Load()
        {
            foreach (var rootItem in GetRootItems())
            {
                rootItem.Load();
            }
        }

        private int _tickCounter = 0;

        protected override void Update()
        {
            // TODO - This is test code to move the first root view every 150 ticks
            _tickCounter++;

            if (_tickCounter == 150)
            {
                _tickCounter = 0;
                var firstRootItem = GetRootItems().FirstOrDefault();

                if (firstRootItem != null)
                {
                    var position = firstRootItem.Position;
                    firstRootItem.Position = new Position(position.X + 100, position.Y + 100);
                }
                
            }

            foreach (var rootItem in GetRootItems())
            {
                rootItem.Measure(new Size(Resolution.Width, Resolution.Height));
            }

            foreach (var rootItem in GetRootItems())
            {
                rootItem.Update();
            }
        }

        public IEnumerable<int> GetDrawOrder()
        {
            foreach (var rootID in _rootIDs)
            {
                var rootItem = _itemByID[rootID];

                foreach (var id in GetDrawOrder(rootID, rootItem))
                {
                    yield return id;
                }
            }
        }

        private IEnumerable<int> GetDrawOrder(int id, IUIItem item)
        {
            if (item.IsVisible)
            {
                yield return id;

                if (_childIDSetByID.TryGetValues(id, out IEnumerable<int> childIDs))
                {
                    foreach (var childID in childIDs)
                    {
                        var childItem = _itemByID[childID];

                        foreach (var grandChildID in GetDrawOrder(childID, childItem))
                        {
                            yield return grandChildID;
                        }
                    }
                }
            }
        }

        public void Draw()
        {
            foreach (var rootItem in GetRootItems())
            {
                rootItem.Draw();
            }
        }

        private void DrawRecursively(IUIItem item)
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
        }
    }
}
