using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TakoEngine.GameObjects;
using Brush = TakoEngine.GameObjects.Brush;

namespace SauceEditor.Controls
{
    /// <summary>
    /// Interaction logic for PropertyPanel.xaml
    /// </summary>
    public partial class PropertyPanel : DockingLibrary.DockableContent
    {
        private GameObject _gameObject;
        private Brush _brush;

        public PropertyPanel()
        {
            InitializeComponent();
        }

        public void SetGameObject(GameObject gameObject)
        {
            _brush = null;
            _gameObject = gameObject;
        }

        public void SetBrush(Brush brush)
        {
            _gameObject = null;
            _brush = brush;
        }
    }
}
