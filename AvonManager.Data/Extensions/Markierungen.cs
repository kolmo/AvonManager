using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace AvonManager.Data
{
    public partial class Markierungen
    {
        #region Fields
        private bool _isSelected;
        private bool _isAssignedToArtikel;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets the IsSelected.
        /// </summary>
        /// <value>
        /// The IsSelected.
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
        /// Gets or sets the IsAssigned.
        /// </summary>
        /// <value>
        /// The IsAssigned.
        /// </value>
        public bool IsAssignedToArtikel
        {
            get { return _isAssignedToArtikel; }
            set
            {
                if (_isAssignedToArtikel != value)
                {
                    _isAssignedToArtikel = value;
                }
            }
        }

        #endregion Properties

        #region Public methods
        public void SetIsAssigned(bool value)
        {
            _isAssignedToArtikel = value;
        }
        #endregion Public methods

    }
}
