using System;
using System.Windows;

namespace AvonManager
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(Exception e)
        {
            InitializeComponent();
            if (e != null)
            {
                ErrorTextBox.Text = e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace;
            }
        }

        public ErrorWindow(Uri uri)
        {
            InitializeComponent();
            if (uri != null)
            {
                ErrorTextBox.Text = "Seite nicht gefunden: \"" + uri.ToString() + "\"";
            }
        }

        public ErrorWindow(string message, string details)
        {
            InitializeComponent();
            ErrorTextBox.Text = message + Environment.NewLine + Environment.NewLine + details;
        }

        public ErrorWindow(string title, string message, string details)
        {
            InitializeComponent();
            CancelButton.Visibility = System.Windows.Visibility.Visible;
            LabelText.Text = "Details";
            this.Title = title ?? string.Empty;
            IntroductoryText.Text = message ?? string.Empty;
            ErrorTextBox.Text = details ?? string.Empty;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}