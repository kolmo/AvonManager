using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AvonManager.Common.Controls
{
    /// <summary>
    /// Interaction logic for SearchStarterControl.xaml
    /// </summary>
    public partial class SearchStarterControl : UserControl
    {
        public SearchStarterControl()
        {
            InitializeComponent();
            image.SetBinding(Image.StyleProperty, new Binding("ImageStyle") { Source = this });
            textBoxSearch.SetBinding(TextBox.TextProperty, new Binding("SearchString")
            {
                Source = this,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            buttonStart.SetBinding(Button.CommandProperty, new Binding("StartSearchCommand") { Source = this });
            buttonReset.SetBinding(Button.CommandProperty, new Binding("ResetSearchCommand") { Source = this });
        }

        public Style ImageStyle
        {
            get { return (Style)GetValue(ImageStyleProperty); }
            set { SetValue(ImageStyleProperty, value); }
        }

        public static readonly DependencyProperty ImageStyleProperty =
            DependencyProperty.Register("ImageStyle", typeof(Style), typeof(SearchStarterControl), new PropertyMetadata(null));

        public string SearchString
        {
            get { return (string)GetValue(SearchStringProperty); }
            set { SetValue(SearchStringProperty, value); }
        }

        public static readonly DependencyProperty SearchStringProperty =
            DependencyProperty.Register("SearchString", typeof(string), typeof(SearchStarterControl), new PropertyMetadata(null));

        public ICommand StartSearchCommand
        {
            get { return (ICommand)GetValue(StartSearchCommandProperty); }
            set { SetValue(StartSearchCommandProperty, value); }
        }

        public static readonly DependencyProperty StartSearchCommandProperty =
            DependencyProperty.Register("StartSearchCommand", typeof(ICommand), typeof(SearchStarterControl), new PropertyMetadata(null));


        public ICommand ResetSearchCommand
        {
            get { return (ICommand)GetValue(ResetSearchCommandProperty); }
            set { SetValue(ResetSearchCommandProperty, value); }
        }
        public static readonly DependencyProperty ResetSearchCommandProperty =
            DependencyProperty.Register("ResetSearchCommand", typeof(ICommand), typeof(SearchStarterControl), new PropertyMetadata(null));
    
    }
}
