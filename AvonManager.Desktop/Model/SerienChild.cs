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

using AvonManager.Data;
using Microsoft.Practices.Prism.Mvvm;

namespace AvonManager.Model
{
    public class SerienChild : BindableBase
    {
        #region Fields
        private Serien _serie;
        private bool _isChild;
        #endregion Fields


        #region Properties
        public string Name { get { return _serie != null ? _serie.Name : string.Empty; } }
        public Serien Serie { get { return _serie; } }
        /// <summary>
        /// Gets or sets the IsChild.
        /// </summary>
        /// <value>
        /// The IsChild.
        /// </value>
        public bool IsChild
        {
            get { return _isChild; }
            set
            {
                if (_isChild != value)
                {
                    _isChild = value;
                    OnPropertyChanged(() => this.IsChild);
                }
            }
        }
        #endregion Properties

        #region Constructors
        public SerienChild(Serien s)
        {
            _serie = s;
        }
        #endregion Constructors

        #region Public methods
        public void SetSelected(bool isChild)
        {
            _isChild = isChild;
        }
        #endregion Public methods
          
    }
}
