using SauceEditorCore.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditorCore.Models.Libraries
{
    public class LibraryInfo
    {
        public enum SortStyles
        {
            Added,
            Alphabetical,
            Size,
            CreationTimes,
            WriteTimes,
            AccessTimes,
            Type
        }

        private List<ComponentInfo> _componentInfos = new List<ComponentInfo>();

        public void AddComponent(IComponent component) => _componentInfos.Add(new ComponentInfo(component));

        public int Count() => _componentInfos.Count;

        public ComponentInfo GetInfoAt(int index) => _componentInfos[index];

        public void RefreshAll()
        {
            foreach (var componentInfo in _componentInfos)
            {
                componentInfo.Refresh();
            }
        }

        public IEnumerable<ComponentInfo> OrderBy(SortStyles sortStyle, bool isDescending = false)
        {
            switch (sortStyle)
            {
                case SortStyles.Added:
                    return isDescending ? _componentInfos.Reverse<ComponentInfo>() : _componentInfos;
                case SortStyles.Alphabetical:
                    return isDescending ? _componentInfos.OrderByDescending(c => c.Name) : _componentInfos.OrderBy(c => c.Name);
                case SortStyles.Size:
                    return isDescending ? _componentInfos.OrderByDescending(c => c.FileSize) : _componentInfos.OrderBy(c => c.FileSize);
                case SortStyles.CreationTimes:
                    return isDescending ? _componentInfos.OrderByDescending(c => c.CreationTime) : _componentInfos.OrderBy(c => c.CreationTime);
                case SortStyles.WriteTimes:
                    return isDescending ? _componentInfos.OrderByDescending(c => c.Name) : _componentInfos.OrderBy(c => c.Name);
                case SortStyles.AccessTimes:
                    return isDescending ? _componentInfos.OrderByDescending(c => c.Name) : _componentInfos.OrderBy(c => c.Name);
            }

            throw new NotImplementedException("Could not handle SortStyle " + sortStyle);
        }

        public void Clear() => _componentInfos.Clear();
    }
}
