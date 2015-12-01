using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace AvonManager.Desktop
{
    /// <summary>
    /// Interaktionslogik für Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : Window
    {
        IRegionManager _regionManager;
        public Shell()
        {
            InitializeComponent();
        }
        public Shell(IUnityContainer container)
            :this()
        {
            _regionManager = container.Resolve<IRegionManager>();
        }
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void ApplicationNameTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var uri = new Uri("HomeView", UriKind.Relative);
            _regionManager.RequestNavigate(AvonManager.Common.RegionNames.MainRegion, uri);
        }
    }
}
