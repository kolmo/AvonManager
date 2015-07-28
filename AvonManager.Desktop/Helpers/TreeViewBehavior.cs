using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AvonManager.Helpers
{
    public class TreeViewBehavior : Behavior<TreeView>
    {


        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(TreeViewBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(AssociatedObject_SelectedItemChanged);
        }

        void AssociatedObject_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = AssociatedObject.SelectedItem;
        }
    }
}
