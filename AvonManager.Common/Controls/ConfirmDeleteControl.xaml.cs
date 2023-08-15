using AvonManager.Common.Base;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AvonManager.Common.Controls
{
    /// <summary>
    /// Interaction logic for ConfirmDeleteControl.xaml
    /// </summary>
    public partial class ConfirmDeleteControl : UserControl,  IInteractionRequestAware
    {
        private DeleteConfirmation _deleteConfirmation;
        public ConfirmDeleteControl()
        {
            InitializeComponent();
        }

        public Action FinishInteraction
        {
            get; set;
        }

        public INotification Notification
        {
            get
            {
                return _deleteConfirmation;
            }

            set
            {
                if (value is DeleteConfirmation)
                {
                    _deleteConfirmation = value as DeleteConfirmation;
                    textBox.Text = _deleteConfirmation.Content.ToString();
                }
            }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            _deleteConfirmation.Confirmed = true;
            FinishInteraction();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            _deleteConfirmation.Confirmed = false;
            FinishInteraction();
        }
    }
}
