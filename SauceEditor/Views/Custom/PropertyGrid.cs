using PropertyChanged;
using SauceEditor.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace SauceEditor.Views.Custom
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SauceEditor.Views"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SauceEditor.Views;assembly=SauceEditor.Views"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ImageButtonList/>
    ///
    /// </summary>
    //[DoNotNotify]
    public class PropertyGrid : Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid
    {
        //public ObservableCollection<IImageButton> ImageButtons { get; set; } 

        public PropertyGrid()
        {
            //ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            //ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
            /*ImageButtons = new ObservableCollection<IImageButton>();

            ImageButtons.Add(new TestImageButton("Test 01"));
            ImageButtons.Add(new TestImageButton("Test 02"));
            ImageButtons.Add(new TestImageButton("Test 03"));

            ItemsSource = ImageButtons;*/
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            
        }
        
        /*protected override void OnPropertyDefinitionsChanged(PropertyDefinitionCollection oldValue, PropertyDefinitionCollection newValue)
        {
            base.OnPropertyDefinitionsChanged(oldValue, newValue);
        }

        protected override void OnEditorDefinitionsChanged(EditorDefinitionCollection oldValue, EditorDefinitionCollection newValue)
        {
            base.OnEditorDefinitionsChanged(oldValue, newValue);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }*/

        static PropertyGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGrid), new FrameworkPropertyMetadata(typeof(PropertyGrid)));
        }
    }
}
