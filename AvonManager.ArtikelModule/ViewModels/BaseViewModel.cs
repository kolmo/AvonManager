using System.Collections.Generic;
using System;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;

namespace AvonManager.ArtikelModule.ViewModels
{
    /// <summary>
    /// Basisklasse für alle Viewmodels; implementiert das Änderungsevent und
    /// liefert lokalisierte Texte
    /// </summary>
    public abstract class BaseViewModel : BindableBase
    {
        #region Fields
        private int busyCount = 0;
        private object lockObject = new object();
        private List<DelegateCommand<object>> commandsBag = new List<DelegateCommand<object>>();
        private int _selectedTabItem;
        #endregion

        #region Properties
 
        public DateTime ToggleState
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Gets or sets the filter setzen.
        /// </summary>
        /// <value>
        /// The filter setzen.
        /// </value>
        public DelegateCommand<object> FilterSetzen { get; protected set; }
        public DelegateCommand<object> FilterReset { get; protected set; }
        #endregion
        #region Constructors
        public BaseViewModel()
        {
            FilterSetzen = new DelegateCommand<object>(FilterSetzenAction);
            FilterReset = new DelegateCommand<object>(FilterResetAction);
        }
        #endregion

        #region Public Methods
        protected virtual void FilterSetzenAction(object obj)
        { }
        protected virtual void FilterResetAction(object obj)
        { }
        public void AddCommand(DelegateCommand<object> command)
        {
            if (!commandsBag.Contains(command))
            {
                commandsBag.Add(command);
            }
        }
        public void CheckCommandsState()
        {
            commandsBag.ForEach(x => x.RaiseCanExecuteChanged());
        }
        /// <summary>
        /// Clears all calls
        /// </summary>
        public void ResetCount()
        {
            busyCount = 0;
            OnPropertyChanged(() => IsBusy);
        }

        /// <summary>
        /// Erhoeht den Busy-Count
        /// </summary>
        public void BusyCountInc()
        {
            busyCount++;
            OnPropertyChanged(() => IsBusy);
        }
        /// <summary>
        /// Vermindert den Busy-Count
        /// </summary>
        public void BusyCountDec()
        {
            lock (lockObject)
            {
                busyCount--;
                if (busyCount < 0)
                {
                    busyCount = 0;
                }
            }
            OnPropertyChanged(() => IsBusy);
            OnPropertyChanged(() => ToggleState);
        }
        /// <summary>
        /// Gibt zurück ob das Viewmodel Busy ist
        /// </summary>
        public bool IsBusy
        {
            get { return busyCount > 0; }

        }
        protected void SubmitChangesAction(object obj)
        {
            //Context.SaveChanges();
        }

        #endregion

        #region Private helper methods

        #endregion
    }
}
