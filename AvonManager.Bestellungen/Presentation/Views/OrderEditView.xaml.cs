using AvonManager.Bestellungen.Presentation.Views;
using System.Windows.Controls;

namespace AvonManager.Bestellungen.Views
{
    /// <summary>
    /// Interaction logic for BestellungEditView.xaml
    /// </summary>
    public partial class OrderEditView : UserControl
    {
        public OrderEditView()
        {
            InitializeComponent();
        }
        public OrderEditView(OrderEditViewModel viewModel)
            :this()
        {
            DataContext = viewModel;
        }
    }
}
