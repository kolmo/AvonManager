using AvonManager.ArtikelModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Controls
{
    /// <summary>
    /// Interaction logic for SerienControl.xaml
    /// </summary>
    public partial class SerienControl : UserControl, IView
    {
        public SerienControl()
        {
            InitializeComponent();
        }
        public SerienControl(SerienAdminViewModel viewmodel):this()
        {
            DataContext = viewmodel;
        }
    }
}
