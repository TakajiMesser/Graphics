using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using DockingLibrary;

namespace DockingLibraryDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : System.Windows.Window
    {
        private PropertyWindow propertyWindow = new PropertyWindow();
        private ExplorerWindow explorerWindow = new ExplorerWindow();
        private ListWindow listWindow = new ListWindow();

        internal static RecentFiles files = new RecentFiles();

        public MainWindow()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

            InitializeComponent();
        }



        private void OnLoaded(object sender, EventArgs e)
        {
            dockManager.ParentWindow = this;
            dockManager2.ParentWindow = this;

            //if (!string.IsNullOrEmpty(Properties.Settings.Default.DockingLayoutState))
            //    dockManager2.RestoreLayoutFromXml(Properties.Settings.Default.DockingLayoutState,
            //        new DockingLibrary.GetContentFromTypeString(this.GetContentFromTypeString));
            //else
            {
                //Show PropertyWindow docked to the top border
                propertyWindow.DockManager = dockManager2;
                propertyWindow.Show(Docks.Left);

                //Show ExplorerWindow docked to the right border as default
                explorerWindow.DockManager = dockManager2;
                explorerWindow.Show();

                //Show ListWindow in documents pane
                listWindow.DockManager = dockManager2;
                listWindow.Show();
            }
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Properties.Settings.Default.DockingLayoutState = dockManager.GetLayoutAsXml();
            Properties.Settings.Default.Save();
        }


        private void ShowExplorerWindow(object sender, EventArgs e)
        {
            explorerWindow.Show();
            //dockManager.Show(explorerWindow);
        }

        private void ShowOutputWindow(object sender, EventArgs e)
        {
            //dockManager.Show(outputWindow);
        }

        private void ShowPropertyWindow(object sender, EventArgs e)
        {
            propertyWindow.Show();
            //dockManager.Show(propertyWindow);
        }

        private void ShowListWindow(object sender, EventArgs e)
        {
            listWindow.Show();
            //dockManager.Show(listWindow);
        }

        bool ContainsDocument(string title)
        {
            foreach (DockingLibrary.DocumentContent doc in dockManager.Documents)
                if (string.Compare(doc.Title, title, true) == 0)
                    return true;
            return false;
        }

        private void NewDocument(object sender, EventArgs e)
        {
            string title = "Document";
            int i = 1;
            while (ContainsDocument(title + i.ToString()))
                i++;

            DocWindow doc = new DocWindow();
            doc.DockManager = dockManager;
            doc.Title = title + i.ToString();
            doc.Show();

            /*DocWindow doc2 = new DocWindow();
            doc2.DockManager = dockManager2;
            doc2.Title = title + i.ToString();
            doc2.Show();*/

            files.Add(new RecentFile(doc.Title, "PATH" + doc.Title, doc.Title.Length*i));
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }


        private DockingLibrary.DockableContent GetContentFromTypeString(string type)
        {
            if (type == typeof(PropertyWindow).ToString())
                return propertyWindow;
            else if (type == typeof(ExplorerWindow).ToString())
                return explorerWindow;
            else if (type == typeof(ListWindow).ToString())
                return listWindow;

            return null;
        }

    }
}