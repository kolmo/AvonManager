using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AvonManager.Common.Controls
{
    /// <summary>
    /// Interaction logic for InitialsSelector.xaml
    /// </summary>
    public partial class InitialsSelector : UserControl
    {
        private ControlContext _viewModel;
        public InitialsSelector()
        {
            InitializeComponent();
            _viewModel = new ControlContext();
            initialsList.DataContext = _viewModel;
            image.SetBinding(Image.StyleProperty, new Binding("ImageStyle") { Source = this });
        }

        public Style ImageStyle
        {
            get { return (Style)GetValue(ImageStyleProperty); }
            set { SetValue(ImageStyleProperty, value); }
        }

        public static readonly DependencyProperty ImageStyleProperty =
            DependencyProperty.Register("ImageStyle", typeof(Style), typeof(InitialsSelector), new PropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(InitialsSelector), new PropertyMetadata(null, new PropertyChangedCallback(ItemsSourcePropertyChanged)));

        private static void ItemsSourcePropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
        {
            InitialsSelector obj = dpo as InitialsSelector;
            if (obj != null)
            {
                IEnumerable iniList = args.NewValue as IEnumerable;
                if (iniList != null)
                {
                    obj._viewModel.InitialsList = iniList;
                }
            }
        }


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(InitialsSelector), new PropertyMetadata(null, new PropertyChangedCallback(CommandParameterPropertyChanged)));

        private static void CommandParameterPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            InitialsSelector obj = dp as InitialsSelector;
            if (obj!= null)
            {
                obj._viewModel.CommandParameter = args.NewValue;
            }
        }

        public ICommand FilterCommand
        {
            get { return (ICommand)GetValue(FilterCommandProperty); }
            set { SetValue(FilterCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterCommandProperty =
            DependencyProperty.Register("FilterCommand", typeof(ICommand), typeof(InitialsSelector), new PropertyMetadata(null, new PropertyChangedCallback(FilterCommandPropertyChanged)));
        private static void FilterCommandPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            InitialsSelector obj = dp as InitialsSelector;
            if (obj!= null)
            {
                obj._viewModel.FilterInitialsCommand = args.NewValue as ICommand;
            }
        }

    }
    internal class ControlContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler tmp = PropertyChanged;
            if (tmp != null)
            {
                tmp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private IEnumerable _initialsList;

        public IEnumerable InitialsList
        {
            get { return _initialsList; }
            set
            {
                if (_initialsList != value)
                {
                    _initialsList = value;
                    OnPropertyChanged(nameof(InitialsList));
                }
            }
        }

        private ICommand _filterInitialsCommand;

        public ICommand FilterInitialsCommand
        {
            get { return _filterInitialsCommand; }
            set
            {
                if (_filterInitialsCommand != value)
                {
                    _filterInitialsCommand = value;
                    OnPropertyChanged(nameof(FilterInitialsCommand));
                }
            }
        }
        private object _commandParameter;

        public object CommandParameter
        {
            get { return _commandParameter; }
            set
            {
                if (_commandParameter != value)
                {
                    _commandParameter = value;
                    OnPropertyChanged(nameof(CommandParameter));
                }
            }
        }

    }
}
