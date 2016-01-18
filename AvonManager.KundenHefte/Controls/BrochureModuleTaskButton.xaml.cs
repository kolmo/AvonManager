using System.Windows.Controls;

namespace AvonManager.KundenHefte.Controls
{
    /// <summary>
    /// Interaction logic for BrochureModuleTaskButton.xaml
    /// </summary>
    public partial class BrochureModuleTaskButton : UserControl
    {
        public BrochureModuleTaskButton()
        {
            InitializeComponent();
        }
        public BrochureModuleTaskButton(BrochureModuleTaskButtonViewModel vm) :this()
        {
            DataContext = vm;
        }
    }
}
