using AvonManager.Bestellungen.Common;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
