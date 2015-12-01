using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class MarkierungenAdminViewModel : BindableBase
    {
        IMarkierungenDataProvider _markierungenDataProvider;
        public MarkierungenAdminViewModel(IMarkierungenDataProvider markierungenDataProvider)
        {
            _markierungenDataProvider = markierungenDataProvider;
        }
        /// <summary>
        /// Gets or sets the Markierungen.
        /// </summary>
        /// <value>
        /// The Markierungen.
        /// </value>
        public ObservableCollection<MarkierungViewModel> Markierungen { get; } = new ObservableCollection<MarkierungViewModel>();

        private MarkierungViewModel _selectedMarkierung;
        /// <summary>
        /// Gets or sets the SelectedMarkierung.
        /// </summary>
        /// <value>
        /// The SelectedMarkierung.
        /// </value>
        public MarkierungViewModel SelectedMarkierung
        {
            get { return _selectedMarkierung; }
            set
            {
                SetProperty(ref _selectedMarkierung, value);
                OnPropertyChanged(nameof(Titel));
                OnPropertyChanged(nameof(Bemerkung));
                OnPropertyChanged(nameof(FarbeARGB));
            }
        }

        /// <summary>
        /// Gets or sets the Titel.
        /// </summary>
        /// <value>
        /// The Titel.
        /// </value>
        public string Titel
        {
            get { return SelectedMarkierung?.Titel; }
            set
            {
                if (SelectedMarkierung != null)
                {
                    SelectedMarkierung.Titel = value;
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
            get { return SelectedMarkierung?.Bemerkung; }
            set
            {
                if (SelectedMarkierung != null)
                {
                    SelectedMarkierung.Bemerkung = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the FarbeARGB.
        /// </summary>
        /// <value>
        /// The FarbeARGB.
        /// </value>
        public int? FarbeARGB
        {
            get { return SelectedMarkierung?.FarbeARGB; }
            set
            {
                if (SelectedMarkierung != null)
                {
                    SelectedMarkierung.FarbeARGB = value;
                }
            }
        }

        public async void LoadData(int entitaetId)
        {
            var liste = await _markierungenDataProvider.ListAllMarkierungen((EntitaetDto)entitaetId);
            Markierungen.Clear();
            foreach (var item in liste)
            {
                MarkierungViewModel vm = new MarkierungViewModel(item, _markierungenDataProvider);
                Markierungen.Add(vm);
            }
            SelectedMarkierung = Markierungen.FirstOrDefault();
        }
    }
}
