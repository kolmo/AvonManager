using AvonManager.BusinessObjects;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvonManager.ArtikelModule.ViewModels
{
    public class SerienAdminViewModel : BindableBase
    {
        private struct BackingFields
        {
            public int SerieId;
            public string Name;
            public string Bemerkung;
        }
        private BackingFields bFields;
        private BackingFields clone;
        private bool _isInitializing = false;
        ISerienDataProvider _serienDataProvider;
        public SerienAdminViewModel(ISerienDataProvider serienDataProvider)
        {
            _serienDataProvider = serienDataProvider;
            AddNewSerieCommand = new DelegateCommand(AddNewSerieAction);
            DeleteSerieCommand = new DelegateCommand(DeleteSerieAction);
        }
        public ObservableCollection<SerieDto> Serien { get; } = new ObservableCollection<SerieDto>();

        private SerieDto _selectedSerie;
        public SerieDto SelectedSerie
        {
            get { return _selectedSerie; }
            set
            {
                _isInitializing = true;
                _selectedSerie = value;
                InitProperties();
                _isInitializing = false;
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
            get { return bFields.Name; }
            set { SetProperty(ref bFields.Name, value); }

        }


        /// <summary>
        /// Gets or sets the Bemerkung.
        /// </summary>
        /// <value>
        /// The Bemerkung.
        /// </value>
        public string Bemerkung
        {
            get { return bFields.Bemerkung; }
            set { SetProperty(ref bFields.Bemerkung, value); }
        }


        public ICommand AddNewSerieCommand { get; }
        public ICommand DeleteSerieCommand { get; }

        public async void LoadData()
        {
            var liste = await _serienDataProvider.ListAllSerien();
            Serien.Clear();
            foreach (var item in liste)
            {
                Serien.Add(item);
            }
            SelectedSerie = Serien.FirstOrDefault();
        }
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (!_isInitializing)
                SaveSerie();
            return ok;
        }
        private void InitProperties()
        {
            if (_selectedSerie!= null)
            {
                bFields.SerieId = _selectedSerie.SerienId;
                Name = _selectedSerie.Name;
                Bemerkung = _selectedSerie.Bemerkung;
                clone = bFields;
            }
        }

        private void SaveSerie()
        {
            if (_selectedSerie!= null)
            {
                _selectedSerie.Name = Name;
                _selectedSerie.Bemerkung = Bemerkung;
                _serienDataProvider.SaveSerie(_selectedSerie);
            }
        }
        private void AddNewSerieAction()
        {
            var neueSerie = new BusinessObjects.SerieDto { Name = "Neue Serie" };
            neueSerie.SerienId = _serienDataProvider.AddSerie(neueSerie);
            Serien.Insert(0, neueSerie);
            SelectedSerie = neueSerie;
        }
        private void DeleteSerieAction()
        {
            if (SelectedSerie != null)
            {
                _serienDataProvider.DeleteSerie(SelectedSerie.SerienId);
                Serien.Remove(SelectedSerie);
                SelectedSerie = Serien.FirstOrDefault();
            }
        }
    }
}
