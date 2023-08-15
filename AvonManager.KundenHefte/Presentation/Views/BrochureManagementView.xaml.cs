using AvonManager.Common;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AvonManager.KundenHefte.Presentation.Views
{
    /// <summary>
    /// Interaction logic for BrochureManagementView.xaml
    /// </summary>
    public partial class BrochureManagementView : UserControl
    {
        IRegionManager _regionManager;
        bool _placeHolderShown;
        public BrochureManagementView()
        {
            InitializeComponent();
            Loaded += BrochureManagementView_Loaded;
        }
        public BrochureManagementView(IRegionManager rm):this()
        {
            _regionManager = rm;
        }
        private void BrochureManagementView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_placeHolderShown)
            {
                var workSpaceUri = new Uri("NoSelectionPlaceHolderView", UriKind.Relative);
                _regionManager.RequestNavigate(RegionNames.BrochureDetailsRegion, workSpaceUri);
                _placeHolderShown = true;
            }
        }
    }
}
