using System.Windows;

namespace NoteBar.Toolbar.MVVM
{
    public class BindingProxy : Freezable
    {
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        protected override Freezable CreateInstanceCore() => new BindingProxy();
    }
}
