using SauceEditorCore.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditorCore.Helpers
{
    public static class PathInfoHelper
    {
        public static IEnumerable<IPathInfo> OrderBy(this IEnumerable<IPathInfo> values, PathSortStyles pathSortStyle, bool isDescending = false)
        {
            switch (pathSortStyle)
            {
                case PathSortStyles.Added:
                    return isDescending ? values.Reverse<IPathInfo>() : values;
                case PathSortStyles.Alphabetical:
                    return isDescending ? values.OrderByDescending(c => c.Name) : values.OrderBy(c => c.Name);
                case PathSortStyles.Size:
                    return isDescending ? values.OrderByDescending(c => c.FileSize) : values.OrderBy(c => c.FileSize);
                case PathSortStyles.CreationTimes:
                    return isDescending ? values.OrderByDescending(c => c.CreationTime) : values.OrderBy(c => c.CreationTime);
                case PathSortStyles.WriteTimes:
                    return isDescending ? values.OrderByDescending(c => c.LastWriteTime) : values.OrderBy(c => c.LastWriteTime);
                case PathSortStyles.AccessTimes:
                    return isDescending ? values.OrderByDescending(c => c.LastAccessTime) : values.OrderBy(c => c.LastAccessTime);
            }

            throw new NotImplementedException("Could not handle PathSortStyle " + pathSortStyle);
        }

        public static bool NeedsRefresh(this PathSortStyles pathSortStyle)
        {
            switch (pathSortStyle)
            {
                case PathSortStyles.Added:
                    return false;
                case PathSortStyles.Alphabetical:
                    return false;
                case PathSortStyles.Size:
                    return true;
                case PathSortStyles.CreationTimes:
                    return true;
                case PathSortStyles.WriteTimes:
                    return true;
                case PathSortStyles.AccessTimes:
                    return true;
            }

            throw new NotImplementedException("Could not handle PathSortStyle " + pathSortStyle);
        }

        public static bool NeedsRecursiveRefresh(this PathSortStyles pathSortStyle)
        {
            switch (pathSortStyle)
            {
                case PathSortStyles.Added:
                    return false;
                case PathSortStyles.Alphabetical:
                    return false;
                case PathSortStyles.Size:
                    return true;
                case PathSortStyles.CreationTimes:
                    return false;
                case PathSortStyles.WriteTimes:
                    return false;
                case PathSortStyles.AccessTimes:
                    return false;
            }

            throw new NotImplementedException("Could not handle PathSortStyle " + pathSortStyle);
        }
    }
}
