using Microsoft.Expression.Encoder.Devices;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WebcamControl;
using System;
using System.Linq;
using AvonManager.Common.Base;
using System.Collections.ObjectModel;

namespace AvonManager.Common.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WebCamClientControl : UserControl, IInteractionRequestAware
    {
        private TakePictureConfirmation _confirmation;
        private Collection<EncoderDevice> _videoDevices;
        public WebCamClientControl()
        {
            InitializeComponent();
            _videoDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);

            WebcamCtrl.FrameRate = 30;
            var audioDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);
            if (audioDevices?.Count > 0)
            {
                WebcamCtrl.AudioDevice = audioDevices[0];
            }
        }

        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                if (value is TakePictureConfirmation)
                {
                    _confirmation = value as TakePictureConfirmation;
                }
            }
        }

        public Action FinishInteraction
        {
            get; set;
        }

        private void TakeSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
           WebcamCtrl.TakeSnapshot();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            _confirmation.Confirmed = false;
            FinishInteraction();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            _confirmation.Confirmed = true;
            _confirmation.ImageData = WebcamCtrl.ImageBytes;
            FinishInteraction();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_videoDevices?.Any() == true)
            {
                WebcamCtrl.VideoDevice = _videoDevices.First();
                WebcamCtrl.StartPreview();
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            WebcamCtrl.StopPreview();
        }
    }
}
