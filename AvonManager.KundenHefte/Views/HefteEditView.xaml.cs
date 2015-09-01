using AvonManager.Interfaces;
using System;
using System.Windows.Controls;

namespace AvonManager.KundenHefte.Views
{
    /// <summary>
    /// Interaction logic for HefteEditView.xaml
    /// </summary>
    public partial class HefteEditView : UserControl, ITitleView
    {
        public HefteEditView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "Heftdetails";
            }
        }
    }
}
