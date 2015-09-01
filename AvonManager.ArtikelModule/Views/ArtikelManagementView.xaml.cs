using AvonManager.Interfaces;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Interaction logic for ArtikelManagementView.xaml
    /// </summary>
    public partial class ArtikelManagementView : UserControl, ITitleView
    {
        public ArtikelManagementView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "Artikel-Management";
            }
        }
    }
}
