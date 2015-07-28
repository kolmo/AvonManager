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
    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();
        }


        // Wenn während der Navigation ein Fehler auftritt, zeigen Sie ein Fehlerfenster an

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            //about.Show();
        }
    }
}