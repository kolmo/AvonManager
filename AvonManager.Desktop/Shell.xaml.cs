using AvonManager.Common.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.ComponentModel.Composition;
using System.Globalization;
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
        IEventAggregator _eventAggregator;
        public Shell()
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.MainWindowBounds))
            {
                try
                {
                    Rect restoreBounds = Rect.Parse(Properties.Settings.Default.MainWindowBounds);
                    this.Left = restoreBounds.Left;
                    this.Top = restoreBounds.Top;
                    this.Width = restoreBounds.Width;
                    this.Height = restoreBounds.Height;
                }
                catch (Exception)
                {                  
                }
            }
        }
        public Shell(IUnityContainer container)
            :this()
        {
            _regionManager = container.Resolve<IRegionManager>();
            _eventAggregator = container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<ModuleChangedEvent>().Subscribe(ModuleChangedEventHandler);
 
        }

        private void ModuleChangedEventHandler(ModuleChangedEventArgs obj)
        {
            textBlockModuleTitle.Text = obj.ModuleTitle;
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void ApplicationNameTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var uri = new Uri("HomeView", UriKind.Relative);
            _regionManager.RequestNavigate(Common.RegionNames.MainRegion, uri);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.MainWindowBounds = this.RestoreBounds.ToString(CultureInfo.InvariantCulture);
            Properties.Settings.Default.Save();
        }
    }
}
