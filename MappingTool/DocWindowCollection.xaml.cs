using DockingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MappingTool
{
    /// <summary>
    /// Interaction logic for DocWindowCollection.xaml
    /// </summary>
    public partial class DocWindowCollection : DockableContent
    {
        public DocWindowCollection()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            DockManager.ParentWindow = this;
        }

        private void OnClosing(object sender, EventArgs e)
        {
            //Properties.Settings.Default.DockingLayoutState = DockManager.GetLayoutAsXml();
            //Properties.Settings.Default.Save();
        }

        private bool ContainsDocument(string title)
        {
            foreach (var document in DockManager.Documents)
            {
                if (string.Compare(document.Title, title, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddDocument()
        {
            var titleBuilder = new StringBuilder("Document");

            int i = 1;
            while (ContainsDocument(titleBuilder.ToString() + i))
            {
                i++;
            }
            titleBuilder.Append(i);

            var doc = new DocWindow()
            {
                DockManager = DockManager,
                Title = titleBuilder.ToString()
            };
            
            doc.Show();

            //var dockablePane = doc.ContainerPane as DockablePane;
            //dockablePane.ChangeState(PaneState.TabbedDocument);
        }
    }
}
