using AvonManager.Interfaces;
using System.Windows.Controls;
using System;

namespace AvonManager.Bestellungen.Views
{
    /// <summary>
    /// Interaction logic for BestellungManagementView.xaml
    /// </summary>
    public partial class BestellungManagementView : UserControl, ITitleView
    {
        public BestellungManagementView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "Bestellungen";
            }
        }
    }
}
