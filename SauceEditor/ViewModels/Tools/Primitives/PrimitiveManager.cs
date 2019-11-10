using System.Collections.Generic;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public class PrimitiveManager
    {
        private List<Primitive> _primitives = new List<Primitive>();

        private void BuildPrimitives()
        {
            _primitives.Clear();
            _primitives.Add(new BoxPrimitive());
            _primitives.Add(new ConePrimitive());
            _primitives.Add(new CylinderPrimitive());
            _primitives.Add(new SpherePrimitive());
        }

        public IEnumerable<Primitive> GetPrimitives()
        {
            if (_primitives.Count == 0)
            {
                BuildPrimitives();
            }

            return _primitives;
        }
    }
}
