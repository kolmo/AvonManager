using AvonManager.Interfaces;
using AvonManager.KundenHefte.Views.Kunden;
using System;
using System.Windows.Controls;

namespace AvonManager.KundenHefte.Views
{
    /// <summary>
    /// Interaction logic for KundenEditView.xaml
    /// </summary>
    public partial class KundenEditView : UserControl
    {
        public KundenEditView()
        {
            InitializeComponent();
        }
        public KundenEditView(KundenEditViewModel viewModel)
            :this()
        {
            DataContext = viewModel;
        }
    }
}
