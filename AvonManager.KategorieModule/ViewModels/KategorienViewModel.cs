using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Mvvm;
using AvonManager.Interfaces;
using AvonManager.BusinessObjects;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;


namespace AvonManager.KategorieModule.ViewModels
{
    public class KategorienViewModel : BindableBase
    {
        #region Private fields
        IKategorieProvider _dataProvider;
        private ObservableCollection<KategorieViewModel> _alleKategorien;
        private bool _isDataLoaded = false;
        #endregion

        #region Konstruktion
        public KategorienViewModel(IKategorieProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _alleKategorien = new ObservableCollection<KategorieViewModel>();
            DeleteCommand = new DelegateCommand<KategorieViewModel>(DeleteCurrentKategorie);

        }
        #endregion

        public async void LoadData()
        {
            if (!_isDataLoaded)
            {
                var result = await _dataProvider.ListAllKategorien();
                foreach (Kategorie kategorie in result)
                {
                    KategorieViewModel vm = new KategorieViewModel(kategorie, _dataProvider);
                    AlleKategorien.Add(vm);
                }
                OnPropertyChanged(() => this.AlleKategorien);
                _isDataLoaded = true;
            }
        }

        #region Properties
        /// <summary>
        /// Gets or sets the AlleKategorien.
        /// </summary>
        /// <value>
        /// The AlleKategorien.
        /// </value>
        public ObservableCollection<KategorieViewModel> AlleKategorien
        {
            get { return _alleKategorien; }
            set
            {
                if (_alleKategorien != value)
                {
                    _alleKategorien = value;
                    OnPropertyChanged(() => this.AlleKategorien);
                }
            }
        }
        public DelegateCommand<KategorieViewModel> DeleteCommand { get; private set; }

        #endregion

        #region Public Methods

        #endregion
        #region Private methods
        private void DeleteCurrentKategorie(KategorieViewModel kategorie)
        {
            if (kategorie != null)
            {
                _dataProvider.DeleteKategorie(kategorie.Kategorie);
                AlleKategorien.Remove(kategorie);
            }
        }

        #endregion
        //#region Fields
        //private Kategorien _selectedKategorie;
        //private IList<SerienChild> _serienChildren;
        //#endregion Fields

        //#region Properties
        //public int SelectedKatTabItem { get; set; }
        //public int SelectedSerieTabItem { get; set; }
        //public IEnumerable<SerienChild> SortedSerienListe
        //{
        //    get
        //    {
        //        return _serienChildren != null ? _serienChildren.OrderBy(x => x.IsChild).ThenBy(x => x.Name) : null;
        //    }
        //}
        //public IEnumerable<SerienChild> ChildSerienListe
        //{
        //    get
        //    {
        //        return _serienChildren != null ? _serienChildren.Where(x => x.IsChild).OrderBy(x => x.Name) : null;
        //    }
        //}
        //public string SerienQuery { get { return "GetSerien"; } }
        //public string KategorienQuery { get { return "GetKategorien"; } }
        //public int LoadSize { get { return 100; } }
        ///// <summary>
        ///// Gets or sets the SelectedSerie.
        ///// </summary>
        ///// <value>
        ///// The SelectedSerie.
        ///// </value>
        //public Serien SelectedSerie
        //{
        //    get { return _selectedSerie; }
        //    set
        //    {
        //        if (_selectedSerie != value)
        //        {
        //            _selectedSerie = value;
        //            PrepareSerieChildZuordnungen();
        //            OnPropertyChanged(() => this.SelectedSerie);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the SelectedKategorie.
        ///// </summary>
        ///// <value>
        ///// The SelectedKategorie.
        ///// </value>
        //public Kategorien SelectedKategorie
        //{
        //    get { return _selectedKategorie; }
        //    set
        //    {
        //        if (_selectedKategorie != value)
        //        {
        //            _selectedKategorie = value;
        //            OnPropertyChanged(() => this.SelectedKategorie);
        //        }
        //    }
        //}


        //#endregion Properties

        //#region Constructors
        ///// <summary>
        ///// Initializes a new instance of the SerienViewModel class.
        ///// </summary>
        //public SerienViewModel()
        //{

        //}

        //#endregion Constructors

        //#region Public methods
        //public void LoadedData()
        //{
        //    LoadSerienCallback();
        //}
        //#endregion Public methods

        //#region Private helper methods
        //private void PrepareSerieChildZuordnungen()
        //{
        //    if (SelectedSerie == null || _serienChildren == null)
        //    {
        //        return;
        //    }
        //    foreach (SerienChild child in _serienChildren)
        //    {
        //        if (child.Serie.Parent == SelectedSerie.SerienId)
        //        {
        //            child.SetSelected(true);
        //        }
        //        else
        //        {
        //            child.SetSelected(false);
        //        }
        //    }
        //    OnPropertyChanged(() => this.SortedSerienListe);
        //    OnPropertyChanged(() => this.ChildSerienListe);
        //}

        //private void LoadSerienCallback()
        //{
        //    _serienChildren = new List<SerienChild>();
        //    foreach (Serien item in Context.Seriens)
        //    {
        //        SerienChild child = new SerienChild(item);
        //        child.PropertyChanged += new PropertyChangedEventHandler(child_PropertyChanged);
        //        _serienChildren.Add(child);
        //    }
        //}

        //void child_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (SelectedSerie == null)
        //    {
        //        return;
        //    }
        //    SerienChild child = sender as SerienChild;
        //    if (child != null && e.PropertyName == "IsChild")
        //    {
        //        child.Serie.Parent = child.IsChild ? new int?(SelectedSerie.SerienId) : null;
        //        CheckCommandsState();
        //    }
        //}
        //#endregion Private helper methods

    }
}