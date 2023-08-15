using AvonManager.BusinessObjects;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Prism.Mvvm;
using System;

namespace AvonManager.KundenHefte.Presentation.Views
{
    public class HeftKundeViewModel : BindableBase
    {
        private KundeDto _kunde;
        private HeftDto _heft;
        private HeftKundeDto _heftKunde;
        private IHefteDataProvider _dataProvider;

        public HeftKundeViewModel(KundeDto kunde, HeftDto heft, HeftKundeDto heftKunde, IHefteDataProvider provider, bool isAssigned)
        {
            _kunde = kunde;
            _heft = heft;
            _heftKunde = heftKunde;
            _dataProvider = provider;
            _received = isAssigned;
            _receivedAt = _heftKunde.ReceivedAt;
            _hasOrdered = _heftKunde.HasOrdered;
        }

        public string Brochure
        { get { return _heft.Titel; } }
        private bool _received;

        /// <summary>
        /// Gets or sets the Received.
        /// </summary>
        /// <value>
        /// The Received.
        /// </value>
        public bool Received
        {
            get { return _received; }
            set
            {
                SetProperty(ref _received, value);
                AddOrDeleteData();
            }
        }

        private void SaveData()
        {
            _heftKunde.HasOrdered = HasOrdered;
            _heftKunde.ReceivedAt = ReceivedAt;
            _dataProvider.SaveHeftKunde(_heftKunde);
        }

        private void AddOrDeleteData()
        {
            try
            {
                if (Received)
                {
                    _dataProvider.AddHeftKunde(_heftKunde);
                }
                else
                {
                    _dataProvider.DeleteHeftKunde(_heftKunde);
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
            }
        }

        /// <summary>
        /// Gets or sets the Customer.
        /// </summary>
        /// <value>
        /// The Customer.
        /// </value>
        public string Customer
        {
            get { return _kunde.Nachname; }
        }

        private DateTime? _receivedAt;

        /// <summary>
        /// Gets or sets the ReceivedAt.
        /// </summary>
        /// <value>
        /// The ReceivedAt.
        /// </value>
        public DateTime? ReceivedAt
        {
            get { return _receivedAt; }
            set
            {
                SetProperty(ref _receivedAt, value);
                SaveData();
            }
        }

        private bool _hasOrdered;

        /// <summary>
        /// Gets or sets the HasOrdered.
        /// </summary>
        /// <value>
        /// The HasOrdered.
        /// </value>
        public bool HasOrdered
        {
            get { return _hasOrdered; }
            set
            {
                SetProperty(ref _hasOrdered, value);
                SaveData();
            }
        }
    }
}