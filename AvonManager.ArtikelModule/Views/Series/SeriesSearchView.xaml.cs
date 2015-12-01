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

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Interaction logic for SeriesSearchView.xaml
    /// </summary>
    public partial class SeriesSearchView : UserControl
    {
        public SeriesSearchView()
        {
            InitializeComponent();
        }
        public SeriesSearchView(SeriesSearchViewModel vm)
            :this()
        {
            DataContext = vm;
        }
    }
}
