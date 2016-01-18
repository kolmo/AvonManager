using AvonManager.Common;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Interaction logic for ArtikelManagementView.xaml
    /// </summary>
    public partial class ArtikelManagementView : UserControl
    {
        IRegionManager _regionManager;
        bool _placeHolderShown;
        public ArtikelManagementView()
        {
            InitializeComponent();
            Loaded += ArtikelManagementView_Loaded;
        }

        public ArtikelManagementView(IRegionManager regionManager)
            :this()
        {
            _regionManager = regionManager;
        }
        private void ArtikelManagementView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_placeHolderShown)
            {
                var workSpaceUri = new Uri("NoSelectionPlaceHolderView", UriKind.Relative);
                _regionManager.RequestNavigate(RegionNames.ArticleDetailsRegion, workSpaceUri);
                _placeHolderShown = true;
            }
        }

    }
}
