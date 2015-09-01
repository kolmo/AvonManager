using AvonManager.ArtikelModule.ViewModels;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Description for SerienView.
    /// </summary>
    public partial class KategoriePage : UserControl, ITitleView
    {
        /// <summary>
        /// Initializes a new instance of the SerienView class.
        /// </summary>
        public KategoriePage(KategoriePageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public string Title
        {
            get
            {
                return "Kategorien & Serien";
            }
        }
    }
}