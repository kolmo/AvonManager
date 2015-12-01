using System;
using System.Collections.Generic;
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

namespace AvonManager.Common.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class EditDeleteCmdControl : UserControl
    {
        public EditDeleteCmdControl()
        {
            InitializeComponent();
        }



        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditCommandProperty =
            DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(EditDeleteCmdControl), new PropertyMetadata(null, new PropertyChangedCallback(EditCommandChanged)));

        private static void EditCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as EditDeleteCmdControl;
            control.buttonEdit.Command = args.NewValue as ICommand;
        }


        public object EditCommandParameter
        {
            get { return (object)GetValue(EditCommandParameterProperty); }
            set { SetValue(EditCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditCommandParameterProperty =
            DependencyProperty.Register("EditCommandParameter", typeof(object), typeof(EditDeleteCmdControl), new PropertyMetadata(null, new PropertyChangedCallback(EditCommandParameterChanged)));

        private static void EditCommandParameterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as EditDeleteCmdControl;
            control.buttonEdit.CommandParameter = args.NewValue;
        }

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(EditDeleteCmdControl), new PropertyMetadata(null, new PropertyChangedCallback(DeleteCommandChanged)));

        private static void DeleteCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as EditDeleteCmdControl;
            control.buttonDelete.Command = args.NewValue as ICommand;
        }



        public object DeleteCommandParameter
        {
            get { return (object)GetValue(DeleteCommandParameterProperty); }
            set { SetValue(DeleteCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteCommandParameterProperty =
            DependencyProperty.Register("DeleteCommandParameter", typeof(object), typeof(EditDeleteCmdControl), new PropertyMetadata(null, new PropertyChangedCallback(DeleteCommandParameterChanged)));

        private static void DeleteCommandParameterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as EditDeleteCmdControl;
            control.buttonDelete.CommandParameter = args.NewValue;
        }

    }
}
