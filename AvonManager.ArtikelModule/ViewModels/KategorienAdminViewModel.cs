using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class KategorienAdminViewModel : BindableBase
    {
        IKategorieProvider _kategorieDataProvider;
        public KategorienAdminViewModel(IKategorieProvider kategorieDataProvider)
        {
            _kategorieDataProvider = kategorieDataProvider;
            AddNewKategorieCommand = new DelegateCommand(AddNewKategorieAction);
            DeleteKategorieCommand = new DelegateCommand(DeleteKategorieAction);
        }
        public ObservableCollection<KategorieViewModel> Kategorien { get; } = new ObservableCollection<KategorieViewModel>();

        private KategorieViewModel _selectedKategorie;
        /// <summary>
        /// Gets or sets the SelectedMarkierung.
        /// </summary>
        /// <value>
        /// The SelectedMarkierung.
        /// </value>
        public KategorieViewModel SelectedKategorie
        {
            get { return _selectedKategorie; }
            set
            {
                SetProperty(ref _selectedKategorie, value);
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Bemerkung));
            }
        }

        /// <summary>
        /// Gets or sets the Titel.
        /// </summary>
        /// <value>
        /// The Titel.
        /// </value>
        public string Name
        {
            get { return SelectedKategorie?.Kategoriename; }
            set
            {
                if (SelectedKategorie != null)
                {
                    SelectedKategorie.Kategoriename = value;
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
            get { return SelectedKategorie?.Bemerkung; }
            set
            {
                if (SelectedKategorie != null)
                {
                    SelectedKategorie.Bemerkung = value;
                }
            }
        }

        public ICommand AddNewKategorieCommand { get; }
        public ICommand DeleteKategorieCommand { get; }

        public async void LoadData()
        {
            var liste = await _kategorieDataProvider.ListAllKategorien();
            Kategorien.Clear();
            foreach (var item in liste)
            {
                KategorieViewModel vm = new KategorieViewModel(item, _kategorieDataProvider);
                Kategorien.Add(vm);
            }
            SelectedKategorie = Kategorien.FirstOrDefault();
        }
        
        private void AddNewKategorieAction()
        {
            var neueKat = new BusinessObjects.KategorieDto { Name = "Neue Kategorie" };
            neueKat.KategorieId = _kategorieDataProvider.AddKategorie(neueKat);
            KategorieViewModel vm = new KategorieViewModel(neueKat, _kategorieDataProvider);
            Kategorien.Insert(0, vm);
            SelectedKategorie = vm;
        }
        private void DeleteKategorieAction()
        {
            if (SelectedKategorie!= null)
            {
                _kategorieDataProvider.DeleteKategorie(SelectedKategorie.KategorieId);
                Kategorien.Remove(SelectedKategorie);
                SelectedKategorie = Kategorien.FirstOrDefault();
            }
        }
    }
}
