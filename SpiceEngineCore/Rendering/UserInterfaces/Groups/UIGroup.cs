using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.UserInterfaces.Layers;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.UserInterfaces.Groups
{
    public class UIGroup : UIItem, IUIGroup
    {
        private List<IUIItem> _children = new List<IUIItem>();

        public IEnumerable<IUIItem> GetChildren() => _children;

        public override void Load()
        {
            
        }

        public override UIQuadSet Measure()
        {
            return new UIQuadSet();
        }

        public override void Update()
        {

        }

        public override void Draw()
        {
            
        }

        public virtual IUIGroup Duplicate() => new UIGroup();
    }
}
