using System.Windows.Controls;
using System;
using Microsoft.Practices.Prism.Regions;
using AvonManager.Common;

namespace AvonManager.Bestellungen.Views
{
    /// <summary>
    /// Interaction logic for BestellungManagementView.xaml
    /// </summary>
    public partial class BestellungManagementView : UserControl
    {
        IRegionManager _regionManager;
        bool _placeHolderShown;
        public BestellungManagementView()
        {
            InitializeComponent();
            Loaded += BestellungManagementView_Loaded;
        }
        public BestellungManagementView(IRegionManager regionManager) :this()
        {
            _regionManager = regionManager;
        }
        private void BestellungManagementView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_placeHolderShown)
            {
                var workSpaceUri = new Uri("NoSelectionPlaceHolderView", UriKind.Relative);
                _regionManager.RequestNavigate(RegionNames.OrderDetailsRegion, workSpaceUri);
                _placeHolderShown = true;
            }
        }
    }
}
