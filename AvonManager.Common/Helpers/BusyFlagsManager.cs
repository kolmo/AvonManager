using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace AvonManager.Common.Helpers
{
    /// <summary>
    /// Stellt einen Satz von Props und Methoden bereit, zur Bindung an einen BusyIndicator.
    /// Hierzu im Viewmodel eine Instanz namens z.B. 'Mgr' dieser Klasse erstellen und als Prop nach aussen geben.
    /// In der View dann über den Pfad 'Mgr.BusyFlags' eine Liste von Countern
    /// an das IsBusy-Prop des Busyindicators gebunden werden. Der einzelne Wert wird über den 
    /// Converter-Parameter via ValueConverter gefiltert. Der zugehörige ValueConverter ist dieser Klasse angehängt.
    /// </summary>
    public class BusyFlagsManager : BindableBase
    {
        private Dictionary<string, int> _busyFlagsCounters;
        private BusyFlagConverter _converter;
        /// <summary>
        /// Initializes a new instance of the <see cref="BusyFlagsManager"/> class.
        /// </summary>
        /// <param name="indicatorKeys">Ein Array mit String-Werten die die einzelnen BusyIndicators identifizieren.</param>
        //public BusyFlagsManager(string[] indicatorKeys)
        //{
        //    _busyFlagsCounters = new Dictionary<string, int>();
        //    if (indicatorKeys != null && indicatorKeys.Length > 0)
        //    {
        //        foreach (string item in indicatorKeys)
        //        {
        //            if (!String.IsNullOrWhiteSpace(item))
        //            {
        //                _busyFlagsCounters[item] = 0;
        //            }
        //        }
        //    }
        //}
        public BusyFlagsManager(BusyFlagConverter converter)
        {
            _converter = converter;
            _busyFlagsCounters = new Dictionary<string, int>();
        }
        /// <summary>
        /// Gets the busy flags.
        /// </summary>
        /// <value>
        /// The busy flags.
        /// </value>
        public Dictionary<string, int> BusyFlags
        {
            get
            {
                return _busyFlagsCounters;
            }
        }

        #region Public methods
        public bool IsBusy(string key)
        {
            return (bool)_converter.Convert(BusyFlags, null, key, null);
        }

        /// <summary>
        /// Incs the busy flag.
        /// </summary>
        /// <param name="key">The key.</param>
        public void IncBusyFlag(params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is string)
                    {
                        string key = args[i] as string;
                        if (!_busyFlagsCounters.ContainsKey(key))
                        {
                            _busyFlagsCounters[key] = 0;
                        }
                        _busyFlagsCounters[key]++;
                    }
                }
                RaisePropertyChanged(nameof(BusyFlags));
            }
        }
        /// <summary>
        /// Decreases the busy flag.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DecBusyFlag(params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is string)
                    {
                        string key = args[i] as string;
                        if (_busyFlagsCounters.ContainsKey(key) && _busyFlagsCounters[key] > 0)
                        {
                            _busyFlagsCounters[key]--;
                        }
                    }
                }
                RaisePropertyChanged(nameof(BusyFlags));
            }
        }

        /// <summary>
        /// Resets the busyflag.
        /// </summary>
        /// <param name="key">The key.</param>
        public void ResetBusyflag(string key)
        {
            if (_busyFlagsCounters.ContainsKey(key))
            {
                _busyFlagsCounters[key] = 0;
                RaisePropertyChanged(nameof(BusyFlags));
            }
        }
        /// <summary>
        /// Resets all busy flags.
        /// </summary>
        public void ResetAllBusyFlags()
        {
            string[] keys = _busyFlagsCounters.Keys.ToArray();
            foreach (string key in keys)
            {
                _busyFlagsCounters[key] = 0;
            }
            RaisePropertyChanged(nameof(BusyFlags));
        }
        #endregion
    }
    /// <summary>
    /// Ermittelt aus einer Dictionary den zum Parameter passenden BusyCounter und gibt dessen Status>0 je nach TargetType zurück
    /// </summary>
    public class BusyFlagConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool retval = false;
            if (value is Dictionary<string, int> && parameter is string)
            {
                string key = parameter as string;
                Dictionary<string, int> dic = value as Dictionary<string, int>;
                if (dic.ContainsKey(key))
                {
                    retval = dic[key] > 0;
                }
            }
            if (targetType == typeof(Boolean))
            {
                return retval;
            }
            else if (targetType == typeof(Visibility))
            {
                return retval ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return retval;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
