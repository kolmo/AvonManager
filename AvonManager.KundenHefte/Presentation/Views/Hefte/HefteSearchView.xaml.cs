using AvonManager.Interfaces;
using System.Windows.Controls;
using System;
using AvonManager.KundenHefte.ViewModels;

namespace AvonManager.KundenHefte.Views
{
    /// <summary>
    /// Interaction logic for HefteSearchView.xaml
    /// </summary>
    public partial class HefteSearchView : UserControl
    {
        public HefteSearchView()
        {
            InitializeComponent();

        }
        public HefteSearchView(HefteSearchViewModel viewModel):this()
        {
            DataContext = viewModel;
        }

    }
}
