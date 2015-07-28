using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace AvonManager.Data
{
    /// <summary>
    /// Ergänzungsklasse für Hefte
    /// </summary>
    public partial class Hefte
    {
        private DateTime _changeFlag;

        public DateTime ChangeFlag
        {
            get
            {
                return _changeFlag;
            }
            set
            {
                if (_changeFlag!=value)
                {
                    _changeFlag = value;
                }
            }
        }
        #region Public methods

        public void RefreshChangeFlag()
        {
            _changeFlag = DateTime.Now;
        }
      
        #endregion

    }
}
