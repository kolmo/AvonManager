using System;

namespace AvonManager.Model
{
    /// <summary>
    /// Ergaenzungen zur Bestellung-Entitätsklasse
    /// </summary>
    public partial class Bestellung
    {
        public decimal Bestellwert
        {
            get
            {
                decimal wert = 0;
                if (this.Bestelldetails != null)
                {
                    foreach (Bestelldetails detail in Bestelldetails)
                    {
                        if (detail.Menge.HasValue && detail.Einzelpreis.HasValue)
                        {
                            wert += detail.Menge.Value * detail.Einzelpreis.Value;
                        }
                    }
                }
                return wert;
            }
        }

        #region Public Methods
        /// <summary>
        /// Setzt einen Trigger fuer jedes Bestelldetail damit evtl. dynamische Daten aktualisiert werden
        /// </summary>
        public void SetDetailsPropertyChanged(Bestelldetails specialDetail = null)
        {

            if (Bestelldetails != null)
            {
                if (specialDetail == null)
                {
                    foreach (Bestelldetails detail in Bestelldetails)
                    {
                        detail.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(detail_PropertyChanged);
                        detail.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(detail_PropertyChanged);
                    }
                }
                else
                {
                    specialDetail.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(detail_PropertyChanged);
                    specialDetail.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(detail_PropertyChanged);
                }
                Bestelldetails.EntityAdded -= new EventHandler<EntityCollectionChangedEventArgs<Web.Bestelldetails>>(detail_PropertyChanged);
                Bestelldetails.EntityAdded += new EventHandler<EntityCollectionChangedEventArgs<Web.Bestelldetails>>(detail_PropertyChanged);
                Bestelldetails.EntityRemoved -= new EventHandler<EntityCollectionChangedEventArgs<Web.Bestelldetails>>(detail_PropertyChanged);
                Bestelldetails.EntityRemoved += new EventHandler<EntityCollectionChangedEventArgs<Web.Bestelldetails>>(detail_PropertyChanged);
            }
        }

        #endregion
        void detail_PropertyChanged(object sender, System.EventArgs e)
        {
            RaisePropertyChanged("Bestellwert");
            RaisePropertyChanged("Bestelldetails");
        }
    }
}
