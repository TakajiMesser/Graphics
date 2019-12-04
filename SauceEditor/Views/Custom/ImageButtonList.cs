using SauceEditor.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            /*var container = element as ListBoxItem;
            var imageButton = item as IImageButton;

            if (imageButton != null && imageButton.SelectCommand != null)
            {
                imageButton.SelectCommand.Executed += (s, args) =>
                {
                    SelectedItem = item;
                };
                //imageButton.SelectCommand = 
            }*/
        }

        static ImageButtonList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButtonList), new FrameworkPropertyMetadata(typeof(ImageButtonList)));
        }   
    }

    public interface IImageButton
    {
        string Name { get; }
        BitmapSource Icon { get; }
        RelayCommand SelectCommand { get; }
        RelayCommand OpenCommand { get; }
        RelayCommand DragCommand { get; }
    }

    public abstract class ImageButton : IImageButton
    {
        private RelayCommand _selectCommand;
        private RelayCommand _openCommand;
        private RelayCommand _dragCommand;

        public ImageButton(string name) => Name = name;

        public string Name { get; set; }
        public BitmapSource Icon { get; set; }

        
        public virtual RelayCommand SelectCommand => (_selectCommand = _selectCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
        
        public virtual RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
        
        public virtual RelayCommand DragCommand => (_dragCommand = _dragCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
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
