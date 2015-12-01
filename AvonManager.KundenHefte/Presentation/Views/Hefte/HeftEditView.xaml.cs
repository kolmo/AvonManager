using AvonManager.Interfaces;
using System;
using System.Windows.Controls;

namespace AvonManager.KundenHefte.Views
{
    /// <summary>
    /// Interaction logic for HefteEditView.xaml
    /// </summary>
    public partial class HeftEditView : UserControl
    {
        public HeftEditView()
        {
            InitializeComponent();
        }
        public HeftEditView(ViewModels.HeftEditViewModel viewModel)
            :this()
        {
            this.DataContext = viewModel;
        }
     
    }
}
