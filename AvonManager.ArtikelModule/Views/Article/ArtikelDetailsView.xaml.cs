using AvonManager.Interfaces;
using System;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Interaction logic for ArtikelDetailsView.xaml
    /// </summary>
    public partial class ArtikelDetailsView : UserControl
    {
        public ArtikelDetailsView(ViewModels.ArtikelDetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
      
    }
}
