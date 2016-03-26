using System.Windows;

namespace MVVMBase.Interactivity
{
    public class MessageBoxCommand : TriggerAction<DependencyObject>
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty
            .Register(
                "Text",
                typeof(string),
                typeof(MessageBoxCommand),
                new PropertyMetadata(""));


        protected override void Invoke(object parameter)
        {
            MessageBox.Show(Text);
        }
    }
}