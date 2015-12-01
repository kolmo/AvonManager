using AvonManager.ArtikelModule.ViewModels;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Controls
{
    /// <summary>
    /// Interaction logic for ArtikelModuleTaskButton.xaml
    /// </summary>
    public partial class KategorieModuleTaskButton : UserControl
    {
        public KategorieModuleTaskButton()
        {
            InitializeComponent();
        }
        public KategorieModuleTaskButton(KategorieModulTaskButtonViewModel viewModel)
            :this()
        {
            this.DataContext = viewModel;
        }
    }
}
