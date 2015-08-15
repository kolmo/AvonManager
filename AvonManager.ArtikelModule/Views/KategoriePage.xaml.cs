using AvonManager.ArtikelModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Description for SerienView.
    /// </summary>
    public partial class KategoriePage : UserControl, IView
    {
        /// <summary>
        /// Initializes a new instance of the SerienView class.
        /// </summary>
        public KategoriePage(KategoriePageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }    
    }
}