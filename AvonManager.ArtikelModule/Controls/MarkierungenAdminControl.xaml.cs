using AvonManager.ArtikelModule.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Controls
{
    public partial class MarkierungenAdminControl : UserControl
    {
        public MarkierungenAdminControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MarkierungenAdminControl_Loaded);
        }
        public MarkierungenAdminControl(MarkierungenAdminViewModel viewModel) :this()
        {
            DataContext = viewModel;
        }
        void MarkierungenAdminControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MarkierungenAdminViewModel)
            {
                (DataContext as MarkierungenAdminViewModel).LoadData(1);
            }
        }
    }
}
