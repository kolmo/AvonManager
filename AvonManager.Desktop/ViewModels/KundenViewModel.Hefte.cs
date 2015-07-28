using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Threading;
using AvonManager.Model;
using AvonManager.Data;
using Microsoft.Practices.Prism.Commands;

namespace AvonManager.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public partial class KundenViewModel : BaseViewModel
    {
        #region Fields
        private Timer _executeFilterTimer;
        private bool _heftHasChanges;
        private int _selectedHeftEditTab;
        private bool _isAddingNewItem = false;
        private int? _sucheJahr;
        #endregion Fields

        #region Properties

        public int? SucheJahr
        {
            get { return _sucheJahr; }
            set
            {
                if (_sucheJahr != value)
                {
                    _sucheJahr = value;
                    BeginFiltering(dueTime: 50);
                }
            }
        }
        /// <summary>
        /// Gets or sets the SelectedHeftEditTab.
        /// </summary>
        /// <value>
        /// The SelectedHeftEditTab.
        /// </value>
        public int SelectedHeftEditTab
        {
            get { return _selectedHeftEditTab; }
            set
            {
                if (_selectedHeftEditTab != value)
                {
                    _selectedHeftEditTab = value;
                    OnPropertyChanged(() => this.SelectedHeftEditTab);
                }
            }
        }

        public IList<string> JahrListe { get; set; }
        public int? SelectedJahr { get; set; }
        public int SelectedHeftTabItem { get; set; }
        public string HefteQueryName { get { return "GetHefte"; } }
        public DelegateCommand<object> AddHeftToClipboard { get; private set; }
        public DelegateCommand<object> ShowHeftDetailsCommand { get; set; }
        public DelegateCommand<object> DeleteCurrentHeftKunden { get; set; }


        private Hefte _currentHeft;
        /// <summary>
        /// Gets or sets the CurrentHeft.
        /// </summary>
        /// <value>
        /// The CurrentHeft.
        /// </value>
        public Hefte CurrentHeft
        {
            get { return _currentHeft; }
            set
            {
                if (_currentHeft != value)
                {
                    _currentHeft = value;
                    PrepareKundenzuordnungen();
                    if (_isAddingNewItem)
                    {
                        _isAddingNewItem = false;
                        //_currentHeft.Jahr = DateTime.Now.Year;
                    }
                    OnPropertyChanged(() => this.CurrentHeft);
                }
            }
        }

        public ObservableCollection<Kunden> SortedKundenListe { get; private set; }
        public DelegateCommand<string> SetJahrFilterCommand { get; private set; }

        #endregion


        #region Overrides
        partial void OnLoadedKunden()
        {
            SetKundePropertyChanged();
        }
        partial void OnInit()
        {
            AddHeftToClipboard = new DelegateCommand<object>(AddHeftToClipboardAction, x => { return CurrentHeft != null; });
            AddCommand(AddHeftToClipboard);
            SetJahrFilterCommand = new DelegateCommand<string>(SetJahrFilterAction);
            ShowHeftDetailsCommand = new DelegateCommand<object>(x =>
            {
                SelectedHeftTabItem = 1;
                OnPropertyChanged(() => this.SelectedHeftTabItem);
            },
                   x => { return true; });
            DeleteCurrentHeftKunden = new DelegateCommand<object>(DeleteCurrentHeftKundenAction);
            _executeFilterTimer = new Timer(RaiseFilterCallback);
        }

        #endregion

        #region Public methods
        public void LoadedHefteComplete(object sender, EventArgs args)
        {
            //if (args.Error != null)
            //{
            //    MessageBox.Show(args.Error.Message);
            //    args.MarkErrorAsHandled();
            //}
            //else
            //{
            //    BusyCountDec();
            //    // Todo Filter und Sorterproblem bei neuem Item lösen!!
            //    PrepareJahresFilterListe();
            //    PrepareKundenzuordnungen();
            //}
        }

        public void AddingNewHeft()
        {
            _isAddingNewItem = true;
        }
        /// <summary>
        /// Deletes the current heft kunden.
        /// </summary>
        private void DeleteCurrentHeftKundenAction(object obj)
        {
            //if (CurrentHeft != null)
            //{
            //    while (CurrentHeft.Kunden.Count > 0)
            //    {
            //        CurrentHeft.Kunden.Remove(CurrentHeft.Kunden.ElementAt(0));
            //    }
            //    Context.Heftes.Remove(CurrentHeft);
            //}
        }
        public void CancelEditHeft()
        {
            //if (CurrentHeft != null && CurrentHeft.HasChanges)
            //{
            //    ((IEditableObject)CurrentHeft).CancelEdit();
            //}
        }
        #endregion

        #region Class override methods
        protected override void FilterResetAction(object obj)
        {
            SucheJahr = null;
            RaiseFilterChanged();
        }
        #endregion Class override methods

        #region Private helper methods
        //private void DeleteKundenBeforeHeftAction(SubmitOperation so)
        //{
        //    if (so.Error == null)
        //    {
        //        Context.Heftes.Remove(CurrentHeft);
        //    }
        //    else
        //    {
        //        TreatError(so);
        //    }
        //}
        private void PrepareJahresFilterListe()
        {
            IList<string> initialList = new List<string> { "*" };
            //JahrListe = initialList.Union(Context.Heftes.Select(x => x.Jahr.ToString()).Distinct().OrderBy(x => x)).ToList();
            OnPropertyChanged(() => JahrListe);
        }
        private void AddHeftToClipboardAction(object obj)
        {
            if (CurrentHeft != null)
            {
                _clipboardMgr.AddObjectToClipboard(CurrentHeft);
            }
        }
        partial void OnKundeChanged()
        {
            SetKundePropertyChanged();
            PrepareJahresFilterListe();
        }
        private void PrepareKundenzuordnungen()
        {
            if (CurrentHeft == null)
            {
                return;
            }
            List<Kunden> verteilerListe = new List<Kunden>();
            //foreach (Hefte_x_Kunden hxk in CurrentHeft.Kunden)
            //{
            //    Context.Kundens.Where(x => x.KundenId == hxk.KundenId).ToList().ForEach(x =>
            //        {
            //            x.SetSelected(true);
            //            x.SetErhalten(hxk.Erhalten);
            //            x.SetHatBestellt(hxk.HatBestellt);
            //            verteilerListe.Add(x);
            //        });
            //}
            //List<Kunden> liste = Context.Kundens.Except(verteilerListe).Where(x => x.Inaktiv != true && x.BekommtHeft == true).ToList();
            //liste.ForEach(
            //                 item =>
            //                 {
            //                     item.SetSelected(false);
            //                     item.SetErhalten(null);
            //                     item.SetHatBestellt(false);
            //                     verteilerListe.Add(item);
            //                 });
            SortedKundenListe = new ObservableCollection<Kunden>(verteilerListe.OrderBy(x => x.IsSelected).ThenBy(x => x.DisplayName));
            OnPropertyChanged(() => this.SortedKundenListe);
        }
        private void SetKundePropertyChanged()
        {
            //foreach (var kunde in Context.Kundens.Where(x => x.BekommtHeft == true))
            //{
            //    //kunde.PropertyChanged -= new PropertyChangedEventHandler(dec_PropertyChanged);
            //    //kunde.PropertyChanged += new PropertyChangedEventHandler(dec_PropertyChanged);
            //}
            PrepareKundenzuordnungen();
        }
        private void BeginFiltering(int dueTime = 500)
        {
            _executeFilterTimer.Change(dueTime, Timeout.Infinite);
        }
        private void SetJahrFilterAction(string arg)
        {
            int jahr = 1900;
            if (int.TryParse(arg, out jahr))
                SucheJahr = jahr;
            else
                SucheJahr = null;
        }

        private void RaiseFilterCallback(object obj)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(RaiseFilterChanged);
        }
        private void RaiseFilterChanged()
        {
            OnPropertyChanged(() => SucheJahr);
        }
        void dec_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CurrentHeft == null)
            {
                return;
            }
            Kunden dec = sender as Kunden;
            //Hefte_x_Kunden hxk = CurrentHeft.Kunden.FirstOrDefault(x => x.KundenId == dec.KundenId);
            //if (dec.IsSelected && hxk == null)
            //{
            //    hxk = new Hefte_x_Kunden();
            //    hxk.KundenId = dec.KundenId;
            //    hxk.Erhalten = dec.Erhalten;
            //    hxk.HatBestellt = dec.HatBestellt;
            //    CurrentHeft.Kunden.Add(hxk);
            //}
            //else if (dec.IsSelected && hxk != null)
            //{
            //    hxk.Erhalten = dec.Erhalten;
            //    hxk.HatBestellt = dec.HatBestellt;
            //}
            //else if (!dec.IsSelected && hxk != null)
            //{
            //    CurrentHeft.Kunden.Remove(hxk);
            //}
            //CheckCommandsState();
            //if (new string[] { "IsSelected", "Erhalten", "HatBestellt" }.Contains(e.PropertyName))
            //{
            //    HeftHasChanges = true;
            //}
        }
        #endregion
    }
}