using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace AvonManager
{
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(About_Loaded);
        }

        void About_Loaded(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Avon-Manager Online";
            AutorText.Text = "Autor: Jörg Dalkolmo";
            ContentText.Text = string.Format("Build Version : {0}", Helpers.AssemblyCreationDate.Value.ToString());
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}