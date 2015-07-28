using AvonManager.ArtikelModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Description for ArtikelPage.
    /// </summary>
    public partial class ArtikelPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ArtikelPage class.
        /// </summary>
        public ArtikelPage(ArtikelPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}