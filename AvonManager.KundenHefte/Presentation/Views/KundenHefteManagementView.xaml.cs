using AvonManager.Common;
using Prism.Regions;
using System;
using System.Windows.Controls;

namespace AvonManager.KundenHefte.Views
{
    /// <summary>
    /// Interaction logic for KundenHefteManagementView.xaml
    /// </summary>
    public partial class KundenHefteManagementView : UserControl
    {
        IRegionManager _regionManager;
        bool _placeHolderShown;
        public KundenHefteManagementView()
        {
            InitializeComponent();
            Loaded += KundenHefteManagementView_Loaded;
        }
        public KundenHefteManagementView(IRegionManager rm):this()
        {
            _regionManager = rm;
        }

        private void KundenHefteManagementView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_placeHolderShown)
            {
                var workSpaceUri = new Uri("NoSelectionPlaceHolderView", UriKind.Relative);
                _regionManager.RequestNavigate(RegionNames.CustomerDetailsRegion, workSpaceUri);
                _placeHolderShown = true;
            }
        }

    }
}
