using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SauceEditor.ViewModels.AttachedBehaviors
{
    public class MouseRightButtonDown
    {
        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MouseRightButtonDown), new UIPropertyMetadata(CommandChanged));
        public static DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(MouseRightButtonDown), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value) => target.SetValue(CommandProperty, value);

        public static void SetCommandParameter(DependencyObject target, object value) => target.SetValue(CommandParameterProperty, value);

        public static object GetCommandParameter(DependencyObject target) => target.GetValue(CommandParameterProperty);

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is Control control)
            {
                if (e.NewValue != null && e.OldValue == null)
                {
                    control.MouseRightButtonDown += OnMouseRightButtonDown;
                }
                else if (e.NewValue == null && e.OldValue != null)
                {
                    control.MouseRightButtonDown -= OnMouseRightButtonDown;
                }
            }
        }

        private static void OnMouseRightButtonDown(object sender, RoutedEventArgs e)
        {
            var dependencyObject = sender as DependencyObject;

            var command = (ICommand)dependencyObject.GetValue(CommandProperty);
            var commandParameter = dependencyObject.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
}
