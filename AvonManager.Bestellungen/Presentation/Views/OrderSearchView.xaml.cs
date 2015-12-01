using AvonManager.Interfaces;
using System.Windows.Controls;
using System;
using AvonManager.Bestellungen.Presentation.Views;

namespace AvonManager.Bestellungen.Views
{
    /// <summary>
    /// Interaction logic for BestellungSearchView.xaml
    /// </summary>
    public partial class OrderSearchView : UserControl
    {
        public OrderSearchView()
        {
            InitializeComponent();
        }
        public OrderSearchView(OrderSearchViewModel viewModel)
        : this()
        {
            DataContext = viewModel;
        }
    }
}
