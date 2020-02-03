using System.Collections.Generic;

namespace StarchUICore.Helpers
{
    public static class UIHelper
    {
        public static T GetAncestor<T>(this IElement element) where T : class, IElement
        {
            if (element.Parent != null)
            {
                if (element.Parent is T t)
                {
                    return t;
                }
                else
                {
                    return GetAncestor<T>(element.Parent);
                }
            }

            return null;
        }

        //public static bool IsAbsolute(this UIUnitTypes unitType) => unitType == UIUnitTypes.Pixels;

        /*public static void Measure(IEnumerable<IUIItem> rootItems, Resolution resolution)
        {
            // TODO - For each root item, we need to traverse down each branch until we get to an absolute measurement
            foreach (var rootItem in rootItems)
            {
                rootItem.Measure(new Size(resolution.Width, resolution.Height));
            }
        }*/

        public static void Draw(IEnumerable<IElement> rootElements)
        {
            foreach (var rootElement in rootElements)
            {
                rootElement.Draw();
            }
        }
    }
}
