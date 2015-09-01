using AvonManager.ArtikelModule.ViewModels;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Description for ArtikelPage.
    /// </summary>
    public partial class ArtikelSearchPage : UserControl, ITitleView
    {
        /// <summary>
        /// Initializes a new instance of the ArtikelPage class.
        /// </summary>
        public ArtikelSearchPage(ArtikelSearchPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public string Title
        {
            get
            {
                return "Suche";
            }
        }
    }
}