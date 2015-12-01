using AvonManager.ArtikelModule.ViewModels;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Interaction logic for MarkierungenSelectView.xaml
    /// </summary>
    public partial class MarkierungenSelectView : UserControl
    {
        public MarkierungenSelectView()
        {
            InitializeComponent();
            DataContext = new MarkierungenSelectViewModel();

        }
       
    }
}
