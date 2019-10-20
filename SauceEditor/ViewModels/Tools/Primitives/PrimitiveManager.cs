using SauceEditor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public class PrimitiveManager
    {
        private List<Primitive> _primitives = new List<Primitive>();

        private void BuildPrimitives()
        {
            _primitives.Add(new BoxPrimitive());
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
