using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Windows.Media;

namespace AvonManager.Model
{
    public partial class Artikel
    {
        public int AnzahlVarianten { get { return Varianten != null ? Varianten.Count : 0; } }
        public RelayCommand<object> ClearSerieCommand { get; private set; }
        /// <summary>
        /// Gets the serienname.
        /// </summary>
        /// <value>
        /// The serienname.
        /// </value>
        public string Serienname
        {
            get
            {
                return Serien != null ? Serien.Name : null;
            }
        }
        #region Public methods
        public void AddVariante(ArtikelVarianten variante)
        {
            Varianten.Add(variante);
            RaisePropertyChanged("AnzahlVarianten");
        }
        public void RemoveVariante(ArtikelVarianten variante)
        {
            Varianten.Remove(variante);
            RaisePropertyChanged("AnzahlVarianten");
        }
        #endregion
        #region Override methods
        protected override void OnLoaded(bool isInitialLoad)
        {
            base.OnLoaded(isInitialLoad);
            if (isInitialLoad)
            {
                if (ClearSerieCommand == null)
                {
                    ClearSerieCommand = new RelayCommand<object>(ClearSerie, x => { return Serien != null; });
                }
                if (Varianten!=null)
                {
                    foreach (ArtikelVarianten variante in Varianten)
                    {
                        variante.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(variante_PropertyChanged);
                        variante.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(variante_PropertyChanged);
                    }
                }
            }
        }

        void variante_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
        partial void OnSerienIdChanged()
        {
            RaisePropertyChanged("Serienname");
        }
        #endregion
        #region Private methods
        private void ClearSerie(object obj)
        {
            this.Serien = null;
        }
        #endregion
    }
}
