using SauceEditor.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SauceEditor.ViewModels.AttachedBehaviors
{
    public class MouseDrag
    {
        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MouseDrag), new UIPropertyMetadata(CommandChanged));
        public static DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(MouseDrag), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value) => target.SetValue(CommandProperty, value);

        public static void SetCommandParameter(DependencyObject target, object value) => target.SetValue(CommandParameterProperty, value);

        public static object GetCommandParameter(DependencyObject target) => target.GetValue(CommandParameterProperty);

        private static Point _startPosition;

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is UIElement uiElement)
            {
                if (e.NewValue != null && e.OldValue == null)
                {
                    uiElement.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                    uiElement.PreviewMouseMove += OnPreviewMouseMove;
                }
                else if (e.NewValue == null && e.OldValue != null)
                {
                    uiElement.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                    uiElement.PreviewMouseMove -= OnPreviewMouseMove;
                }
            }
        }

        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPosition = e.GetPosition(null);
        }

        private static void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(null);
            var difference = _startPosition - position;

            if (e.LeftButton == MouseButtonState.Pressed && DragHelper.IsSignificantDrag(difference))
            {
                var dependencyObject = sender as DependencyObject;

                var command = (ICommand)dependencyObject.GetValue(CommandProperty);
                //var commandParameter = control.GetValue(CommandParameterProperty);
                var commandParameter = e.OriginalSource;

                command.Execute(commandParameter);
                //e.Handled = true;
            }
        }
    }
}
