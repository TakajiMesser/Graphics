using SauceEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SauceEditor.Views
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
    public class ImageButtonList : ListBox
    {
        //public ObservableCollection<IImageButton> ImageButtons { get; set; } 

        public ImageButtonList()
        {
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
            /*ImageButtons = new ObservableCollection<IImageButton>();

            ImageButtons.Add(new TestImageButton("Test 01"));
            ImageButtons.Add(new TestImageButton("Test 02"));
            ImageButtons.Add(new TestImageButton("Test 03"));

            ItemsSource = ImageButtons;*/
        }

        static ImageButtonList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButtonList), new FrameworkPropertyMetadata(typeof(ImageButtonList)));
        }
    }

    public class TestImageButton : IImageButton
    {
        public string Name { get; private set; }
        public BitmapSource Icon => null;

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));

        public TestImageButton(string name) => Name = name;
    }

    public abstract class ImageButton
    {
        private RelayCommand _selectCommand;

        public string Name { get; set; }
        public BitmapSource Icon { get; set; }
        
        public RelayCommand OpenCommand { get; set; }

        public RelayCommand SelectCommand => (_selectCommand = _selectCommand ?? new RelayCommand(
            p =>
            {

            },
            p => true
        )); 
    }

    public interface IImageButton
    {
        string Name { get; }
        BitmapSource Icon { get; }
        RelayCommand OpenCommand { get; }
    }

    /*<ListBox ItemsSource="{Binding Children}" ScrollViewer.HorizontalScrollBarVisibility="Disabled">
        <ListBox.ItemTemplate>
            <DataTemplate>
                <Button cb:MouseDoubleClick.Command="{Binding OpenCommand}">
                    <StackPanel Orientation="Vertical">
                        <Image Width="100" Height="100" Source="{Binding Path=PreviewIcon}"/>
                        <TextBlock Text="{Binding Path=PathInfo.Name}"/>
                    </StackPanel>
                </Button>
            </DataTemplate>
        </ListBox.ItemTemplate>
        <ListBox.ItemsPanel>
            <ItemsPanelTemplate>
                <WrapPanel IsItemsHost="True"/>
            </ItemsPanelTemplate>
        </ListBox.ItemsPanel>
    </ListBox>*/
}
