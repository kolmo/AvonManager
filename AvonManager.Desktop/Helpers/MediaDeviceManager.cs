using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using ImageTools.IO.Png;
using ImageTools;
using System.IO;
using GalaSoft.MvvmLight;

namespace AvonManagerOnline.Helpers
{
    /// <summary>
    /// Verwaltet und liefert Zugang zu Media Devices (z. B. Webcam)
    /// </summary>
    public class MediaDeviceManager : ObservableObject
    {

        #region Fields
        private static MediaDeviceManager _instance;
        private CaptureSource _webcam = null;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Die einzige Instanz der Klasse
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static MediaDeviceManager Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MediaDeviceManager();
                }
                return _instance;
            }
        }
        public byte[] PngImageBytes { get; private set; }
        #endregion Properties

        #region Constructors
        private MediaDeviceManager()
        {
            CheckForWebcam();
        }

        private void CheckForWebcam()
        {
            if (CaptureDeviceConfiguration.AllowedDeviceAccess)
            {
                // Todo Hier eine Möglichkeit vorsehen ein bevorzugtes Device anzugeben.
                VideoCaptureDevice defaultVcd = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
                if (defaultVcd != null)
                {
                    _webcam = new CaptureSource();
                    _webcam.VideoCaptureDevice = defaultVcd;
                    _webcam.CaptureImageCompleted += new System.EventHandler<CaptureImageCompletedEventArgs>(captureSource_CaptureImageCompleted);
                }
            }
        }
        #endregion Constructors

        #region Public methods
        public void StartWebcam()
        {
            // Webcam einschalten
            if (_webcam != null && _webcam.State != CaptureState.Started)
            {
                _webcam.Start();
            }
        }
        public void StopWebcam()
        {
            // Webcam ausschalten
            if (_webcam != null && _webcam.State != CaptureState.Stopped)
            {
                _webcam.Stop();
            }
        }
        public void CaptureImageFromWebcam()
        {
            if (_webcam != null)
            {
                // Verify the VideoCaptureDevice is not null and the device is started.
                if (_webcam.VideoCaptureDevice != null && _webcam.State == CaptureState.Started)
                {
                    _webcam.CaptureImageAsync();
                }
            }
        }
        #endregion Public methods

        #region Private helper methods
        void captureSource_CaptureImageCompleted(object sender, CaptureImageCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                WriteableBitmap v = e.Result;
                PngEncoder enc = new PngEncoder();
                using (MemoryStream ms = new MemoryStream())
                {
                    enc.Encode(v.ToImage(), ms);
                    PngImageBytes = ms.ToArray();
                    RaisePropertyChanged(() => this.PngImageBytes);
                }
            }
        }
        #endregion Private helper methods
    }
}
