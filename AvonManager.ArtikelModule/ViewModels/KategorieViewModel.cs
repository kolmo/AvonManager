using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class KategorieViewModel : BindableBase
    {
        private readonly IKategorieProvider _provider;
        private KategorieDto _kategorie;
        public KategorieViewModel(KategorieDto kategorie, IKategorieProvider provider)
        {
            _kategorie = kategorie;
            _provider = provider;
        }

        #region Properties
        public KategorieDto Kategorie { get { return _kategorie; } }
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
        #region Fields
        private bool _isSelected;
        private bool _isAssignedToArtikel;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets or sets the IsSelected.
        /// </summary>
        /// <value>
        /// The IsSelected.
        /// </value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// Gets or sets the IsAssigned.
        /// </summary>
        /// <value>
        /// The IsAssigned.
        /// </value>
        public bool IsAssignedToArtikel
        {
            get { return _isAssignedToArtikel; }
            set
            {
                if (_isAssignedToArtikel != value)
                {
                    _isAssignedToArtikel = value;
                    OnPropertyChanged("IsAssignedToArtikel");
                }
            }
        }

        #endregion Properties

        #region Public methods
        public void SetIsAssigned(bool value)
        {
            _isAssignedToArtikel = value;
        }
        #endregion Public methods
    }
}
