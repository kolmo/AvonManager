using AvonManager.Interfaces;
using System.Windows.Controls;
using System;

namespace AvonManager.Bestellungen.Views
{
    /// <summary>
    /// Interaction logic for BestellungSearchView.xaml
    /// </summary>
    public partial class BestellungSearchView : UserControl, ITitleView
    {
        public BestellungSearchView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "Suche";
            }
        }
    }
}
