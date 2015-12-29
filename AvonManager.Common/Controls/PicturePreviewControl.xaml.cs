using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AvonManager.Common.Controls
{
    /// <summary>
    /// Interaction logic for PicturePreviewControl.xaml
    /// </summary>
    public partial class PicturePreviewControl : UserControl, IInteractionRequestAware
    {
        Notification _notification;
        public PicturePreviewControl()
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
                return _notification;
            }

            set
            {
                _notification = value as Notification;
                byte[] imagedata = _notification.Content as byte[];
              
                if (imagedata?.Length > 0)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(imagedata);
                    bmi.EndInit();
                    image.Source = bmi;
                }
                else
                {
                    image.Source = null;
                }
            }
        }

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FinishInteraction();
        }
    }
}
