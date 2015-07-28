using Microsoft.Practices.Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AvonManager.Helpers
{
    public class InstantCommitBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
        }
        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonUp -= new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
        }
        void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.CommitEdit(DataGridEditingUnit.Cell, false);
        }
    }
    /// <summary>
    /// Stellt ein Command zur Verfügung um sofort mit der Bearbeitung einer Zelle zu beginnen.
    /// </summary>
    public class BeginEditBehavior : Behavior<DataGrid>
    {
        public ICommand BeginEditCommand
        {
            get { return (ICommand)GetValue(BeginEditCommandProperty); }
            set { SetValue(BeginEditCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BeginEditCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BeginEditCommandProperty =
            DependencyProperty.Register("BeginEditCommand", typeof(ICommand), typeof(BeginEditBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
        }
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs args)
        {
            BeginEditCommand = new DelegateCommand<object>(BeginEditAction);
        }
        private void BeginEditAction(object arg)
        {
            if (AssociatedObject.Columns.Count > 0)
            {
                AssociatedObject.CurrentColumn = AssociatedObject.Columns[0];
                AssociatedObject.Focus();
                AssociatedObject.BeginEdit();
            }
        }
    }
}
