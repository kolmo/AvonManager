
namespace AvonManager.Model
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Bestelldetails
    {
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
                if (this.Menge.HasValue && this.Einzelpreis.HasValue)
                {
                    wert = Menge.Value * Einzelpreis.Value;
                }
                return wert;
            }
        }
        partial void OnEinzelpreisChanged()
        {
            RaisePropertyChanged("Detailwert");
        }
        partial void OnMengeChanged()
        {
            RaisePropertyChanged("Detailwert");
        }
    }
}
