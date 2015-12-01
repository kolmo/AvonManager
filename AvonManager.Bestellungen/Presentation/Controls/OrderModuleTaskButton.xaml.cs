using System.Windows.Controls;

namespace AvonManager.Bestellungen.Controls
{
    /// <summary>
    /// Interaction logic for ArtikelModuleTaskButton.xaml
    /// </summary>
    public partial class OrderModuleTaskButton : UserControl
    {
        public OrderModuleTaskButton()
        {
            InitializeComponent();
        }
        public OrderModuleTaskButton(OrderModulTaskButtonViewModel viewModel)
            :this()
        {
            this.DataContext = viewModel;
        }
    }
}
