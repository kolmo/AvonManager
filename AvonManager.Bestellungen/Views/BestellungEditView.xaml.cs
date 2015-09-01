using AvonManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvonManager.Bestellungen.Views
{
    /// <summary>
    /// Interaction logic for BestellungEditView.xaml
    /// </summary>
    public partial class BestellungEditView : UserControl, ITitleView
    {
        public BestellungEditView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "Details";
            }
        }
    }
}
