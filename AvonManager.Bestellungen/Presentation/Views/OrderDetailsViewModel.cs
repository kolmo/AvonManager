using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using AvonManager.Interfaces;

namespace AvonManager.Bestellungen.Presentation.Views
{
    public class OrderDetailsViewModel : BindableBase
    {
        private struct Backingfields
        {
            public string Campagne;
            public int Jahr;
            public int? Seite;
            public string Artikelnummer;
            public int? Menge;
            public string Artikelbeschreibung;
            public string FDG;
            public decimal? Einzelpreis;
        }
        private Backingfields bFields;
        private Backingfields clone;
        private BestelldetailDto _orderDetail;
        private readonly IOrderDataProvider _orderDataProvider;
        public OrderDetailsViewModel(BestelldetailDto orderDetails, IOrderDataProvider orderDataProvider)
        {
            _orderDetail = orderDetails;
            _orderDataProvider = orderDataProvider;
            InitProperties();
        }


        #region Properties
        public int OrderDetailId { get { return _orderDetail.DetailId; } }
        /// <summary>
        /// Gets or sets the Einzelpreis.
        /// </summary>
        /// <value>
        /// The Einzelpreis.
        /// </value>
        public decimal? Einzelpreis
        {
            get { return bFields.Einzelpreis; }
            set
            {
                SetProperty(ref bFields.Einzelpreis, value);
                OnPropertyChanged(nameof(Detailwert));
            }
        }

        /// <summary>
        /// Gets or sets the FDG.
        /// </summary>
        /// <value>
        /// The FDG.
        /// </value>
        public string FDG
        {
            get { return bFields.FDG; }
            set { SetProperty(ref bFields.FDG, value); }
        }

        /// <summary>
        /// Gets or sets the Artikelbeschreibung.
        /// </summary>
        /// <value>
        /// The Artikelbeschreibung.
        /// </value>
        public string Artikelbeschreibung
        {
            get { return bFields.Artikelbeschreibung; }
            set { SetProperty(ref bFields.Artikelbeschreibung, value); }
        }

        /// <summary>
        /// Gets or sets the Seite.
        /// </summary>
        /// <value>
        /// The Seite.
        /// </value>
        public int? Seite
        {
            get { return bFields.Seite; }
            set { SetProperty(ref bFields.Seite, value); }
        }

        /// <summary>
        /// Gets or sets the Artikelnummer.
        /// </summary>
        /// <value>
        /// The Artikelnummer.
        /// </value>
        public string Artikelnummer
        {
            get { return bFields.Artikelnummer; }
            set { SetProperty(ref bFields.Artikelnummer, value); }
        }

        /// <summary>
        /// Gets or sets the Menge.
        /// </summary>
        /// <value>
        /// The Menge.
        /// </value>
        public int? Menge
        {
            get { return bFields.Menge; }
            set
            {
                SetProperty(ref bFields.Menge, value);
                OnPropertyChanged(nameof(Detailwert));
            }
        }

        /// <summary>
        /// Gets or sets the Jahr.
        /// </summary>
        /// <value>
        /// The Jahr.
        /// </value>
        public int Jahr
        {
            get { return bFields.Jahr; }
            set { SetProperty(ref bFields.Jahr, value); }
        }

        /// <summary>
        /// Gets or sets the Campagne.
        /// </summary>
        /// <value>
        /// The Campagne.
        /// </value>
        public string Campagne
        {
            get { return bFields.Campagne; }
            set { SetProperty(ref bFields.Campagne, value); }
        }
        /// <summary>
        /// Wert eines Postens Menge X Einzelpreis
        /// </summary>
        /// <value>
        /// The detailwert.
        /// </value>
        public decimal Detailwert
        {
            get
            {
                decimal wert = 0;
                if (Menge.HasValue && Einzelpreis.HasValue)
                {
                    wert = Menge.Value * Einzelpreis.Value;
                }
                return wert;
            }
        }
        #endregion
        #region Overrides
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            SaveOrderDetail();
            return ok;
        }
        #endregion
        #region Private methods
        private void InitProperties()
        {
            if (_orderDetail!= null)
            {
                bFields.Artikelbeschreibung = _orderDetail.Artikelbeschreibung;
                bFields.Artikelnummer = _orderDetail.Artikelnummer;
                bFields.Campagne = _orderDetail.Campagne;
                bFields.Einzelpreis = _orderDetail.Einzelpreis;
                bFields.FDG = _orderDetail.FDG;
                bFields.Jahr = _orderDetail.Jahr.HasValue ? _orderDetail.Jahr.Value : 1900;
                bFields.Menge = _orderDetail.Menge;
                bFields.Seite = _orderDetail.Seite;
                clone = bFields;
            }
        }
        private void SaveOrderDetail()
        {
            _orderDetail.Artikelbeschreibung = Artikelbeschreibung;
            _orderDetail.Artikelnummer = Artikelnummer;
            _orderDetail.Campagne = Campagne;
            _orderDetail.Einzelpreis = Einzelpreis;
            _orderDetail.FDG = FDG;
            _orderDetail.Jahr = Jahr;
            _orderDetail.Menge = Menge;
            _orderDetail.Seite = Seite;
            _orderDataProvider.UpdateOrderDetail(_orderDetail);
            clone = bFields;
        }

        #endregion
    }
}
