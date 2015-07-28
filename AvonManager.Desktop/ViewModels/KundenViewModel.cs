using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;
using System.Windows;
using System.Windows.Input;
using AvonManager.Helpers;
using AvonManager.Helpers.Messages;
using System;
using AvonManager.Data;
using AvonManager.Model;
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
        private ClipboardManager _clipboardMgr;
        private Kunden _selectedKunde;
        private IList<InitialSelektor> _initials;
        private bool _isAddingNewKundin = false;
        #endregion Fields

        #region Properties

        public IList<InitialSelektor> Initials
        {
            get { return _initials; }
            set
            {
                if (_initials != value)
                {
                    _initials = value;
                    OnPropertyChanged(() => Initials);
                }
            }
        }
        private ICollectionView _kundenView;
        /// <summary>
        /// Gets or sets the KundenView.
        /// </summary>
        /// <value>
        /// The KundenView.
        /// </value>
        public ICollectionView KundenView
        {
            get { return _kundenView; }
            set
            {
                if (_kundenView != value)
                {
                    _kundenView = value;
                    OnPropertyChanged(() => this.KundenView);
                }
            }
        }

        private bool? _nurAktiveKunden = true;
        /// <summary>
        /// Gets or sets the NurInaktiveKunden.
        /// </summary>
        /// <value>
        /// The NurInaktiveKunden.
        /// </value>
        public bool? NurAktiveKunden
        {
            get { return _nurAktiveKunden; }
            set
            {
                if (_nurAktiveKunden != value)
                {
                    _nurAktiveKunden = value;
                    OnPropertyChanged(() => this.NurAktiveKunden);
                }
            }
        }

        private bool _alleKunden;
        /// <summary>
        /// Gets or sets the AlleKunden.
        /// </summary>
        /// <value>
        /// The AlleKunden.
        /// </value>
        public bool AlleKunden
        {
            get { return _alleKunden; }
            set
            {
                if (_alleKunden != value)
                {
                    _alleKunden = value;
                    OnPropertyChanged(() => this.AlleKunden);
                }
            }
        }

        public string QueryName { get { return "GetKunden"; } }
        /// <summary>
        /// Gets or sets the SelectedKunde.
        /// </summary>
        /// <value>
        /// The SelectedKunde.
        /// </value>
        public Kunden SelectedKunde
        {
            get { return _selectedKunde; }
            set
            {
                if (_selectedKunde != value)
                {
                    _selectedKunde = value;
                    if (_isAddingNewKundin)
                    {
                        _selectedKunde.Inaktiv = false;
                        _selectedKunde.BekommtHeft = true;
                    }
                    this.CheckCommandsState();
                    OnPropertyChanged(() => this.SelectedKunde);
                }
            }
        }
        public Kunden CurrentKunde { get { return KundenView.CurrentItem as Kunden; } }
        public ICommand ReloadData { get; set; }
        public DelegateCommand SetKundenFilterCommand { get; set; }
        public DelegateCommand ResetKundenFilterCommand { get; set; }

        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the KundenViewModel class.
        /// </summary>
        public KundenViewModel()
        {
            _clipboardMgr = ServiceLocator.Current.GetInstance<ClipboardManager>();
            SetKundenFilterCommand = new DelegateCommand(SetKundenFilter);
            ResetKundenFilterCommand = new DelegateCommand(ResetKundenFilter);
            OnInit();
        }

        #endregion Constructors

        #region Public methods
        public void PageUnloaded()
        {
            //if (Context.HasChanges && !Context.IsSubmitting)
            //{
            //    Context.RejectChanges();
            //}
        }
        public void OnKundinAdded()
        {
            _isAddingNewKundin = true;
        }

        public void LoadedKundenComplete(object sender, EventArgs args)
        {
            //if (args.Error != null)
            //{
            //    ErrorWindow ew = new ErrorWindow(args.Error);
            //    ew.Show();
            //    args.MarkErrorAsHandled();
            //}
            //else
            //{
            //    BusyCountDec();
            //    KundenView = (Context.Kundens as ICollectionViewFactory).CreateView();
            //    SetKundenFilter();
            //    SetupSorters();
            //    KundenView.MoveCurrentToFirst();
            //    IList<InitialSelektor> initialList = new List<InitialSelektor>();
            //    Context.Kundens.Where(x => !string.IsNullOrWhiteSpace(x.Nachname)).Select(x => x.Nachname.Trim().Substring(0, 1).ToUpper()).Distinct().OrderBy(x => x).ToList().ForEach(x => initialList.Add(new InitialSelektor { Initial = x }));
            //    Initials = initialList;
            //    OnLoadedKunden();
            //}
        }

        #endregion Public methods

        #region Extension Methods
        partial void OnInit();
        partial void OnLoadedKunden();
        partial void OnKundeChanged();
        #endregion
        #region Callbacks/Event Handler
        #endregion
        #region Private helper methods
        private void SetKundenFilter()
        {
            KundenView.Filter = (x) =>
            {
                bool matched = true;
                Kunden k = x as Kunden;
                if (!AlleKunden)
                    if (NurAktiveKunden == true)
                    {
                        matched = (k.Inaktiv == false || k.Inaktiv == null);
                    }
                    else if (NurAktiveKunden == false)
                    {
                        matched = (k.Inaktiv == true);
                    }
                if (Initials != null && Initials.Any(xx => xx.IsSelected))
                {
                    if (!string.IsNullOrWhiteSpace(k.Nachname) && k.Nachname.Length > 0)
                    {
                        matched = matched && Initials.Any(xx => xx.IsSelected && xx.Initial == k.Nachname.Substring(0, 1).ToUpper());
                    }
                    else
                        matched = false;
                }
                return matched;
            };
        }
        private void SetupSorters()
        {
            if (KundenView != null)
            {
                KundenView.SortDescriptions.Clear();
                KundenView.SortDescriptions.Add(new SortDescription("Nachname", ListSortDirection.Ascending));
            }
        }
        private void ResetKundenFilter()
        {
            if (Initials != null)
            {
                Initials.ToList().ForEach(x => x.IsSelected = false);
            }
            AlleKunden = true;
            SetKundenFilter();
        }

        #endregion Private helper methods

    }
}