using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using AvonManager.KundenHefte.ViewModels;
using Prism.Events;
using Prism.Regions;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvonManager.KundenHefte.Views.Kunden
{
    public class KundenEditViewModel : ErrorAwareBaseViewModel, INavigationAware
    {
        private struct BackingFields
        {
            public int KundenId;
            public string Vorname;
            public string Nachname;
            public string Adresse;
            public string Ort;
            public string Postleitzahl;
            public string EmailAdresse;
            public string TelefonPrivat;
            public string TelefonBeruflich;
            public string MobilesTelefon;
            public string Faxnummer;
            public DateTime? Geburtsdatum;
            public string Anmerkungen;
            public bool? BekommtHeft;
            public byte[] Bild;
            public bool Inaktiv;
        }

        private const string LOAD = "LOAD";
        private BackingFields bFields;
        private BackingFields clone;
        private IKundenDataProvider _kundenDataProvider;
        private bool _isInitializing = false;
        private KundeDto _currentCustomer;

        public KundenEditViewModel(IKundenDataProvider kundenDataProvider
            , IEventAggregator eventAggregator
            , BusyFlagsManager bFlagsManager) : base(bFlagsManager, eventAggregator)
        {
            _kundenDataProvider = kundenDataProvider;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Inaktiv.
        /// </summary>
        /// <value>
        /// The Inaktiv.
        /// </value>
        public bool Inaktiv
        {
            get { return bFields.Inaktiv; }
            set { SetProperty(ref bFields.Inaktiv, value); }
        }

        /// <summary>
        /// Gets or sets the Bild.
        /// </summary>
        /// <value>
        /// The Bild.
        /// </value>
        public byte[] Bild
        {
            get { return bFields.Bild; }
            set { SetProperty(ref bFields.Bild, value); }
        }

        /// <summary>
        /// Gets or sets the BekommtHeft.
        /// </summary>
        /// <value>
        /// The BekommtHeft.
        /// </value>
        public bool? BekommtHeft
        {
            get { return bFields.BekommtHeft; }
            set { SetProperty(ref bFields.BekommtHeft, value); }
        }

        /// <summary>
        /// Gets or sets the Anmerkungen.
        /// </summary>
        /// <value>
        /// The Anmerkungen.
        /// </value>
        public string Anmerkungen
        {
            get { return bFields.Anmerkungen; }
            set { SetProperty(ref bFields.Anmerkungen, value); }
        }

        /// <summary>
        /// Gets or sets the Geburtsdatum.
        /// </summary>
        /// <value>
        /// The Geburtsdatum.
        /// </value>
        public DateTime? Geburtsdatum
        {
            get { return bFields.Geburtsdatum; }
            set { SetProperty(ref bFields.Geburtsdatum, value); }
        }

        /// <summary>
        /// Gets or sets the Faxnummer.
        /// </summary>
        /// <value>
        /// The Faxnummer.
        /// </value>
        public string Faxnummer
        {
            get { return bFields.Faxnummer; }
            set { SetProperty(ref bFields.Faxnummer, value); }
        }

        /// <summary>
        /// Gets or sets the MobilesTelefon.
        /// </summary>
        /// <value>
        /// The MobilesTelefon.
        /// </value>
        public string MobilesTelefon
        {
            get { return bFields.MobilesTelefon; }
            set { SetProperty(ref bFields.MobilesTelefon, value); }
        }

        /// <summary>
        /// Gets or sets the TelefonBeruflich.
        /// </summary>
        /// <value>
        /// The TelefonBeruflich.
        /// </value>
        public string TelefonBeruflich
        {
            get { return bFields.TelefonBeruflich; }
            set { SetProperty(ref bFields.TelefonBeruflich, value); }
        }

        /// <summary>
        /// Gets or sets the TelefonPrivat.
        /// </summary>
        /// <value>
        /// The TelefonPrivat.
        /// </value>
        public string TelefonPrivat
        {
            get { return bFields.TelefonPrivat; }
            set { SetProperty(ref bFields.TelefonPrivat, value); }
        }

        /// <summary>
        /// Gets or sets the EmailAdresse.
        /// </summary>
        /// <value>
        /// The EmailAdresse.
        /// </value>
        public string EmailAdresse
        {
            get { return bFields.EmailAdresse; }
            set { SetProperty(ref bFields.EmailAdresse, value); }
        }

        /// <summary>
        /// Gets or sets the Postleitzahl.
        /// </summary>
        /// <value>
        /// The Postleitzahl.
        /// </value>
        public string Postleitzahl
        {
            get { return bFields.Postleitzahl; }
            set { SetProperty(ref bFields.Postleitzahl, value); }
        }

        /// <summary>
        /// Gets or sets the Ort.
        /// </summary>
        /// <value>
        /// The Ort.
        /// </value>
        public string Ort
        {
            get { return bFields.Ort; }
            set { SetProperty(ref bFields.Ort, value); }
        }

        /// <summary>
        /// Gets or sets the Adresse.
        /// </summary>
        /// <value>
        /// The Adresse.
        /// </value>
        public string Adresse
        {
            get { return bFields.Adresse; }
            set { SetProperty(ref bFields.Adresse, value); }
        }

        /// <summary>
        /// Gets or sets the Nachname.
        /// </summary>
        /// <value>
        /// The Nachname.
        /// </value>
        public string Nachname
        {
            get { return bFields.Nachname; }
            set { SetProperty(ref bFields.Nachname, value); }
        }

        /// <summary>
        /// Gets or sets the Vorname.
        /// </summary>
        /// <value>
        /// The Vorname.
        /// </value>
        public string Vorname
        {
            get { return bFields.Vorname; }
            set { SetProperty(ref bFields.Vorname, value); }
        }

        /// <summary>
        /// Gets or sets the KundenId.
        /// </summary>
        /// <value>
        /// The KundenId.
        /// </value>
        public int KundenId
        {
            get { return bFields.KundenId; }
            set { SetProperty(ref bFields.KundenId, value); }
        }

        #endregion Properties

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (!_isInitializing)
                SaveCustomer();
            return ok;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            BusyFlagsMgr.ResetBusyflag(LOAD);
            var kundeParameter = navigationContext.Parameters.FirstOrDefault();
            if (kundeParameter.Value is KundeViewModel)
            {
                int customerId = ((KundeViewModel)kundeParameter.Value).KundeId;
                LoadCurrentKunde(customerId);
            }
            else
            {
                _currentCustomer = new KundeDto();
                InitProperties();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #region Private methods

        private void InitProperties()
        {
            _isInitializing = true;
            KundenId = _currentCustomer.KundenId;
            Vorname = _currentCustomer.Vorname;
            Nachname = _currentCustomer.Nachname;
            Adresse = _currentCustomer.Adresse;
            Postleitzahl = _currentCustomer.Postleitzahl;
            Ort = _currentCustomer.Ort;
            TelefonBeruflich = _currentCustomer.TelefonBeruflich;
            TelefonPrivat = _currentCustomer.TelefonPrivat;
            MobilesTelefon = _currentCustomer.MobilesTelefon;
            Anmerkungen = _currentCustomer.Anmerkungen;
            EmailAdresse = _currentCustomer.EmailAdresse;
            Faxnummer = _currentCustomer.Faxnummer;
            BekommtHeft = _currentCustomer.BekommtHeft;
            Inaktiv = _currentCustomer.Inaktiv ?? false;
            Bild = _currentCustomer.Bild;
            Geburtsdatum = _currentCustomer.Geburtsdatum;
            clone = bFields;
            _isInitializing = false;
        }

        private void SaveCustomer()
        {
            if (_currentCustomer != null)
            {
                _currentCustomer.KundenId = KundenId;
                _currentCustomer.Vorname = Vorname;
                _currentCustomer.Nachname = Nachname;
                _currentCustomer.Adresse = Adresse;
                _currentCustomer.Postleitzahl = Postleitzahl;
                _currentCustomer.Ort = Ort;
                _currentCustomer.Anmerkungen = Anmerkungen;
                _currentCustomer.TelefonBeruflich = TelefonBeruflich;
                _currentCustomer.TelefonPrivat = TelefonPrivat;
                _currentCustomer.MobilesTelefon = MobilesTelefon;
                _currentCustomer.Faxnummer = Faxnummer;
                _currentCustomer.BekommtHeft = BekommtHeft;
                _currentCustomer.Inaktiv = Inaktiv;
                _currentCustomer.Bild = Bild;
                _currentCustomer.Geburtsdatum = Geburtsdatum;
                _currentCustomer.EmailAdresse = EmailAdresse;
                try
                {
                    _kundenDataProvider.SaveKunde(_currentCustomer);
                    clone = bFields;
                    EventAggregator.GetEvent<CustomerChangedEvent>().Publish(new CustomerChangedEventArgs { Customer = _currentCustomer, ChangedType = ChangedType.Update });
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }

        public async void LoadCurrentKunde(int kundenId)
        {
            BusyFlagsMgr.IncBusyFlag(LOAD);
            _isInitializing = true;
            try
            {
                var customers = await _kundenDataProvider.LoadCustomers(new int[] { kundenId });
                _currentCustomer = customers.First();
                InitProperties();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            _isInitializing = false;
            BusyFlagsMgr.DecBusyFlag(LOAD);
        }

        #endregion Private methods
    }
}