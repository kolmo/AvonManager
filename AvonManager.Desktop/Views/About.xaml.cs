using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}