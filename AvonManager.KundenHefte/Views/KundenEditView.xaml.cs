using AvonManager.Interfaces;
using System;
using System.Windows.Controls;

namespace AvonManager.KundenHefte.Views
{
    /// <summary>
    /// Interaction logic for KundenEditView.xaml
    /// </summary>
    public partial class KundenEditView : UserControl, ITitleView
    {
        public KundenEditView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "Kundendetails";
            }
        }
    }
}
