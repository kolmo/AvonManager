using System;

namespace AvonManager.Data
{
    public partial class Kunden
    {
        #region Fields
        private bool _isSelected;
        private DateTime? _erhalten;
        private bool? _hatbestellt;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets the IsAssigned.
        /// </summary>
        /// <value>
        /// The IsAssigned.
        /// </value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the HatBestellt.
        /// </summary>
        /// <value>
        /// The HatBestellt.
        /// </value>
        public bool? HatBestellt
        {
            get { return _hatbestellt; }
            set
            {
                if (_hatbestellt != value)
                {
                    _hatbestellt = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the Erhalten.
        /// </summary>
        /// <value>
        /// The Erhalten.
        /// </value>
        public DateTime? Erhalten
        {
            get { return _erhalten; }
            set
            {
                if (_erhalten != value)
                {
                    _erhalten = value;
                }
            }
        }
        public string DisplayName
        {
            get
            {
                //if (!String.IsNullOrWhiteSpace(Nachname) && !string.IsNullOrWhiteSpace(Vorname))
                //{
                //    return String.Format("{0}, {1}", Nachname, Vorname);
                //}
                //else if (!string.IsNullOrWhiteSpace(Nachname))
                //{
                //    return String.Format("{0}", Nachname);
                //}
                //else if(!string.IsNullOrWhiteSpace(Vorname))
                //{
                //    return String.Format("?, {0}", Vorname);
                //}
                //else
                //{
                //    return "?, ?";
                //}
                return string.Empty;
            }
        }
        #endregion

        #region Public methods
        public void SetSelected(bool selected)
        {
            _isSelected = selected;
        }
        public void SetErhalten(DateTime? date)
        {
            _erhalten = date;
        }
        public void SetHatBestellt(bool? bestellt)
        {
            _hatbestellt = bestellt;
        }
        #endregion Public methods

    }
}
