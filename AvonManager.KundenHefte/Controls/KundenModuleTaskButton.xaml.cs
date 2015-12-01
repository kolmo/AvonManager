using AvonManager.KundenHefte.ViewModels;
using System.Windows.Controls;

namespace AvonManager.KundenHefte.Controls
{
    /// <summary>
    /// Interaction logic for ArtikelModuleTaskButton.xaml
    /// </summary>
    public partial class KundenModuleTaskButton : UserControl
    {
        public KundenModuleTaskButton()
        {
            InitializeComponent();
        }
        public KundenModuleTaskButton(KundenModulTaskButtonViewModel viewModel)
            :this()
        {
            this.DataContext = viewModel;
        }
    }
}
