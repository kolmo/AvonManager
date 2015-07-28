using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.KategorieModule.ViewModels
{
    public class KategorieViewModel : BindableBase
    {
        private readonly IKategorieProvider _provider;
        private Kategorie _kategorie;
        public KategorieViewModel(Kategorie kategorie, IKategorieProvider provider)
        {
            _kategorie = kategorie;
            _provider = provider;
        }

        #region Properties
        public Kategorie Kategorie { get { return _kategorie; } }
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name
        {
            get { return _kategorie.Name; }
            set
            {
                if (_kategorie.Name != value)
                {
                    _kategorie.Name = value;
                    OnPropertyChanged(() => this.Name);
                    SaveKategorie();
                }
            }
        }
        /// <summary>
        /// Gets or sets the Bemerkung.
        /// </summary>
        /// <value>
        /// The Bemerkung.
        /// </value>
        public string Bemerkung
        {
            get { return _kategorie.Bemerkung; }
            set
            {
                if (_kategorie.Bemerkung != value)
                {
                    _kategorie.Bemerkung = value;
                    OnPropertyChanged(() => this.Bemerkung);
                    SaveKategorie();
                }
            }
        }
        /// <summary>
        /// Gets or sets the Logo.
        /// </summary>
        /// <value>
        /// The Logo.
        /// </value>
        public byte[] Logo
        {
            get { return _kategorie.Logo; }
            set
            {
                if (_kategorie.Logo != value)
                {
                    _kategorie.Logo = value;
                    OnPropertyChanged(() => this.Logo);
                    SaveKategorie();
                }
            }
        }
        #endregion

        #region Private Helper Methods
        private void SaveKategorie()
        {
            _provider.SaveKategorie(_kategorie);
        }
        #endregion
    }
}
