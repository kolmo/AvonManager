using AvonManager.ArtikelModule.ViewModels;
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

namespace AvonManager.ArtikelModule.Controls
{
    /// <summary>
    /// Interaction logic for KategorienAdminControl.xaml
    /// </summary>
    public partial class KategorienAdminControl : UserControl
    {
        public KategorienAdminControl()
        {
            InitializeComponent();
        }
        public KategorienAdminControl(KategorienAdminViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
