using OpenTK.Audio.OpenAL;
using SpiceEngineCore.Rendering.UserInterfaces.Views;
using SpiceEngineCore.Sounds;
using System;
using System.IO;

namespace SpiceEngineCore.Helpers
{
    public static class UIHelper
    {
        public static T GetAncestor<T>(this IUIView view) where T : class, IUIView
        {
            if (view.Parent != null)
            {
                if (view.Parent is T t)
                {
                    return t;
                }
                else
                {
                    return GetAncestor<T>(view.Parent);
                }
            }

            return null;
        }
    }
}
