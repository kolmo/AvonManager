using AvonManager.ArtikelModule.ViewModels;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Description for ArtikelPage.
    /// </summary>
    public partial class ArtikelSearchView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ArtikelPage class.
        /// </summary>
        public ArtikelSearchView(ArtikelSearchViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}