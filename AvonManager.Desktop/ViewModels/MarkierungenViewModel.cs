
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Input;
using AvonManager.Helpers.Messages;
using AvonManager.Model;
using AvonManager.BusinessObjects;

namespace AvonManager.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MarkierungenViewModel : BaseViewModel
    {
        #region Fields
        private bool _dataIsLoaded = false;
        private int _entitaetId;
        private enum ActionType
        {
            None = 0,
            Change = 1,
            Delete = 2
        }
        #endregion Fields

        #region Properties

        private string _entityName;
        /// <summary>
        /// Gets or sets the EntityName.
        /// </summary>
        /// <value>
        /// The EntityName.
        /// </value>
        public string EntityName
        {
            get { return _entityName; }
            set
            {
                if (_entityName != value)
                {
                    _entityName = value;
                    OnPropertyChanged(() => this.EntityName);
                }
            }
        }
        private ICollectionView _markierungenView;
        /// <summary>
        /// Gets or sets the MarkierungenView.
        /// </summary>
        /// <value>
        /// The MarkierungenView.
        /// </value>
        public ICollectionView MarkierungenView
        {
            get { return _markierungenView; }
            set
            {
                if (_markierungenView != value)
                {
                    _markierungenView = value;
                    if (_markierungenView != null)
                    {
                        _markierungenView.MoveCurrentToFirst();
                    }
                    OnPropertyChanged(() => this.MarkierungenView);
                }
            }
        }
        public Markierung CurrentMarkierung { get { return MarkierungenView != null ? MarkierungenView.CurrentItem as Markierung : null; } }
        #endregion Properties

        #region Public methods
        /// <summary>
        /// Lädt die Markierungen
        /// </summary>
        /// <param name="entitaetId">The entitaet id.</param>
        public void LoadData(int entitaetId)
        {
            if (!_dataIsLoaded)
            {
                //_entitaetId = entitaetId;
                //Context.Load(Context.GetMarkierungenByEntityTypeQuery(entitaetId), LoadMarkierungenCallback, false);
                //Context.Load(Context.GetEntitaetenQuery(), LoadEntitaetenCallback, false);
            }
        }
        #endregion Public methods

    }
}