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
                    RaisePropertyChanged("ChangeFlag");
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
