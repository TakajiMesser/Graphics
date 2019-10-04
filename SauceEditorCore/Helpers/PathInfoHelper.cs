using System;
using System.Collections;
using System.Collections.Generic;
using SauceEditorCore.Models.Components;
using SpiceEngineCore.Helpers;
using SauceEditorCore.Models.Libraries;
using System.Linq;
using SortStyles = SauceEditorCore.Models.Libraries.LibraryManager.SortStyles;

namespace SauceEditorCore.Helpers
{
    public static class PathInfoHelper
    {
        public static IEnumerable<IPathInfo> OrderBy(this IEnumerable<IPathInfo> values, SortStyles sortStyle, bool isDescending = false)
        {
            switch (sortStyle)
            {
                case SortStyles.Added:
                    return isDescending ? values.Reverse<IPathInfo>() : values;
                case SortStyles.Alphabetical:
                    return isDescending ? values.OrderByDescending(c => c.Name) : values.OrderBy(c => c.Name);
                case SortStyles.Size:
                    return isDescending ? values.OrderByDescending(c => c.FileSize) : values.OrderBy(c => c.FileSize);
                case SortStyles.CreationTimes:
                    return isDescending ? values.OrderByDescending(c => c.CreationTime) : values.OrderBy(c => c.CreationTime);
                case SortStyles.WriteTimes:
                    return isDescending ? values.OrderByDescending(c => c.Name) : values.OrderBy(c => c.Name);
                case SortStyles.AccessTimes:
                    return isDescending ? values.OrderByDescending(c => c.Name) : values.OrderBy(c => c.Name);
            }

            throw new NotImplementedException("Could not handle SortStyle " + sortStyle);
        }
    }
}
