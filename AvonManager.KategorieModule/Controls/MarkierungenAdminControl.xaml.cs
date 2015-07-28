using AvonManager.KategorieModule.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AvonManager.KategorieModule.Controls
{
    public partial class MarkierungenAdminControl : UserControl
    {
        public MarkierungenAdminControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MarkierungenAdminControl_Loaded);
        }

        void MarkierungenAdminControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MarkierungenViewModel)
            {
                //(DataContext as MarkierungenViewModel).LoadData(this.EntitaetId);
            }
        }
        public int EntitaetId { get; set; }
    }
}
