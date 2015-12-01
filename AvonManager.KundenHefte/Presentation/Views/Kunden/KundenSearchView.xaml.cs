using AvonManager.KundenHefte.ViewModels;
using System.Windows.Controls;

namespace AvonManager.KundenHefte.Views
{
    /// <summary>
    /// Interaction logic for KundenSearchView.xaml
    /// </summary>
    public partial class KundenSearchView : UserControl
    {
        public KundenSearchView()
        {
            InitializeComponent();
        }
        public KundenSearchView(KundenSearchViewModel viewModel) :this()
        {
            DataContext = viewModel;
        }
    }
}
