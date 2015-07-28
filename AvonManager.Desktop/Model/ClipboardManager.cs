using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;

namespace AvonManager.Model
{
    /// <summary>
    /// Verwaltet das Clipboard der Anwendung
    /// </summary>
    public class ClipboardManager
    {


        #region Properties
        public ObservableCollection<object> Clipboard { get; private set; }
        #endregion Properties

        #region Constructors
        public ClipboardManager()
        {
            Clipboard = new ObservableCollection<object>();
        }
        #endregion Constructors

        #region Public methods
        /// <summary>
        /// Adds the object to clipboard.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void AddObjectToClipboard(object obj)
        {
            if (obj!=null && !Clipboard.Contains(obj))
            {
                Clipboard.Add(obj);
            }
        }
        /// <summary>
        /// Clears the clipboard.
        /// </summary>
        public void ClearClipboard()
        {
            Clipboard.Clear();
        }
        public IList<T> GetAllOfType<T>() where T: class
        {
            return Clipboard.Where(x => x is T).Cast<T>().ToList();
        }
        #endregion Public methods	
          
    }
}
