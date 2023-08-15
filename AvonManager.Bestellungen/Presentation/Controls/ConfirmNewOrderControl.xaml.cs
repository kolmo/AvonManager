using AvonManager.Bestellungen.Common;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AvonManager.Bestellungen.Controls
{
    /// <summary>
    /// Interaction logic for ConfirmNewOrderControl.xaml
    /// </summary>
    public partial class ConfirmNewOrderControl : UserControl, IInteractionRequestAware
    {
        public ConfirmNewOrderControl()
        {
            InitializeComponent();
        }

        private NewOrderConfirmation _newOrderConfirmation;

        public Action FinishInteraction
        {
            get; set;
        }

        public INotification Notification
        {
            get
            {
                return _newOrderConfirmation;
            }

            set
            {
                if (value is NewOrderConfirmation)
                {
                    _newOrderConfirmation = value as NewOrderConfirmation;
                    textBox.Text = _newOrderConfirmation.Content.ToString();
                }
            }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            _newOrderConfirmation.Confirmed = true;
            FinishInteraction();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            _newOrderConfirmation.Confirmed = false;
            FinishInteraction();
        }
    }
}