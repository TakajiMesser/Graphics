using SpiceEngineCore.Outputs;
using StarchUICore.Attributes;
using System.Collections.Generic;

namespace StarchUICore.Helpers
{
    public static class UIHelper
    {
        public static T GetAncestor<T>(this IUIItem item) where T : class, IUIItem
        {
            if (item.Parent != null)
            {
                if (item.Parent is T t)
                {
                    return t;
                }
                else
                {
                    return GetAncestor<T>(item.Parent);
                }
            }

            return null;
        }

        public static bool IsAbsolute(this UIUnitTypes unitType) => unitType == UIUnitTypes.Pixels;

        public static void Measure(IEnumerable<IUIItem> rootItems, Resolution resolution)
        {
            // TODO - For each root item, we need to traverse down each branch until we get to an absolute measurement
            foreach (var rootItem in rootItems)
            {
                rootItem.Measure(new Size(resolution.Width, resolution.Height));
            }
        }

        public static void Draw(IEnumerable<IUIItem> rootItems)
        {
            foreach (var rootItem in rootItems)
            {
                rootItem.Draw();
            }
        }
    }
}
