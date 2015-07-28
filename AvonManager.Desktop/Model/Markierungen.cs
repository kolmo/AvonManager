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

namespace AvonManager.Model
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
                    RaisePropertyChanged("IsSelected");
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
                    RaisePropertyChanged("IsAssignedToArtikel");
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
