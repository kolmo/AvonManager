using System.Windows.Controls;

namespace AvonManager.ArtikelModule.Views
{
    /// <summary>
    /// Interaction logic for CategoryEditView.xaml
    /// </summary>
    public partial class CategoryEditView : UserControl
    {
        public CategoryEditView()
        {
            InitializeComponent();
        }
        public CategoryEditView(CategoryEditViewModel vm)
            :this()
        {
            DataContext = vm;
        }
    }
}
