using Microsoft.Expression.Encoder.Devices;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;
using AvonManager.Common.Base;
using AvonManager.Common.Helpers;

namespace AvonManager.Common.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WebCamClientControl : UserControl, IInteractionRequestAware
    {
        private TakePictureConfirmation _confirmation;
        EncoderDevice _currentVideoDevice;
        EncoderDevice _currentAudioDevice;
        public WebCamClientControl()
        {
            InitializeComponent();
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
            try
            {
                WebcamCtrl.TakeSnapshot();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
            }
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
            try
            {
                if (_currentAudioDevice == null)
                {
                    _currentAudioDevice = EncoderDevices.FindDevices(EncoderDeviceType.Audio)?.FirstOrDefault();
                    WebcamCtrl.AudioDevice = _currentAudioDevice;
                    Logger.Current.Write($"Verwende AudioDevice '{WebcamCtrl.AudioDevice?.Name}'.");
                }
                if (_currentVideoDevice == null)
                {
                    _currentVideoDevice = EncoderDevices.FindDevices(EncoderDeviceType.Video)?.FirstOrDefault();
                    WebcamCtrl.VideoDevice = _currentVideoDevice;
                    WebcamCtrl.FrameRate = 30;
                    Logger.Current.Write($"Verwende Videodevice '{WebcamCtrl.VideoDevice?.Name}'.");
                }
                if (WebcamCtrl.VideoDevice != null)
                    WebcamCtrl.StartPreview();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                WebcamCtrl.StopPreview();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
            }
        }
    }
}
