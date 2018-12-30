using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using AvonManager.Common.Base;
using System.Linq;

namespace AvonManager.Common.Controls
{
    public partial class FileLoader : UserControl
    {
        private const string CasioPath = @"DCIM\100CASIO";
        public InteractionRequest<TakePictureConfirmation> TakeSnapshotRequest { get; } = new InteractionRequest<TakePictureConfirmation>();
        public InteractionRequest<Notification> PreviewPictureRequest { get; } = new InteractionRequest<Notification>();
        #region Constructors
        public FileLoader()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Public methods
        protected void SetWebcamButtonVisibility(bool isVisible)
        {
            if (isVisible)
            {
                capturePicFromWebcam.Visibility = Visibility.Visible;
            }
            else
            {
                capturePicFromWebcam.Visibility = Visibility.Collapsed;
            }
        }
        #endregion Public methods

        private void openFileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            var removableDrive = DriveInfo.GetDrives().FirstOrDefault(x => x.DriveType == DriveType.Removable);
            if (removableDrive != null)
            {
                var initDir = Path.Combine(removableDrive.RootDirectory.FullName, CasioPath);
                ofd.InitialDirectory = Directory.Exists(initDir) ? initDir : removableDrive.RootDirectory.FullName;
            }
            if (ofd.ShowDialog() == true)
            {
                this.FileData = ReadFileBytes(new FileInfo(ofd.FileName));
            }
        }
        /// <summary>
        /// Liest eine Datei als Byte-Array
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Das Byte-Array</returns>
        private byte[] ReadFileBytes(FileInfo file)
        {
            byte[] data = null;
            if (file != null)
            {
                using (FileStream fs = new FileStream(file.FullName, FileMode.Open))
                {
                    data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                }
            }
            return data;
        }

        #region Dependency Props
        public bool EnableWebcam
        {
            get { return (bool)GetValue(EnableWebcamProperty); }
            set
            {
                SetValue(EnableWebcamProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for EnableWebcam.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableWebcamProperty =
            DependencyProperty.Register("EnableWebcam", typeof(bool), typeof(FileLoader), new PropertyMetadata(false, EnableWebcamChanged));
        private static void EnableWebcamChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            FileLoader loader = dp as FileLoader;
            if (loader != null)
            {
                loader.SetWebcamButtonVisibility((bool)args.NewValue);
            }
        }

        /// <summary>
        /// Erhaelt die Daten der Webcam
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data
        {
            get { return (byte[])GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(byte[]), typeof(FileLoader), new PropertyMetadata(null, (d, args) =>
            {
                (d as FileLoader).FileData = args.NewValue as byte[];
            }));

        public byte[] FileData
        {
            get { return (byte[])GetValue(FileDataProperty); }
            set { SetValue(FileDataProperty, value); }
        }
        public static readonly DependencyProperty FileDataProperty =
            DependencyProperty.Register("FileData", typeof(byte[]), typeof(FileLoader), new PropertyMetadata(null));
        public object ButtonContent
        {
            get { return (object)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(object), typeof(FileLoader), new PropertyMetadata(null, (d, args) =>
            {
                (d as FileLoader).openFileDialog.Content = args.NewValue;
            }
            ));

        #endregion
        private void capturePicFromWebcam_Click(object sender, RoutedEventArgs e)
        {
            if (EnableWebcam)
            {
                TakePictureConfirmation confirmation = new TakePictureConfirmation() { Title = "Bild aufnehmen" };
                TakeSnapshotRequest.Raise(confirmation, TakeSnapshotAction);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetWebcamButtonVisibility(this.EnableWebcam);
        }
        private void TakeSnapshotAction(TakePictureConfirmation confirmation)
        {
            if (confirmation.Confirmed && confirmation.ImageData != null)
            {
                FileData = confirmation.ImageData;
            }
        }

        private void deletePic_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Wirklich löschen?", "Bild löschen", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.OK)
            {
                this.FileData = null;
            }
        }

        private void preview_Click(object sender, RoutedEventArgs e)
        {
            Notification previewPictureNotif = new Notification() { Title = "Vorschau", Content = FileData };
            PreviewPictureRequest.Raise(previewPictureNotif, null);
        }
    }
}
