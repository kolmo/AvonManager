using AvonManager.Interfaces;
using System.Windows.Controls;
using System;

namespace AvonManager.KundenHefte.Views
{
    /// <summary>
    /// Interaction logic for HefteSearchView.xaml
    /// </summary>
    public partial class HefteSearchView : UserControl, ITitleView
    {
        public HefteSearchView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "Heftsuche";
            }
        }
    }
}
