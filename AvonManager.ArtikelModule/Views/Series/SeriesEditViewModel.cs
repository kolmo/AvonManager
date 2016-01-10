using AvonManager.BusinessObjects;
using AvonManager.Common.Base;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvonManager.ArtikelModule.Views
{
    public class SeriesEditViewModel : ErrorAwareBaseViewModel, INavigationAware
    {
        private struct BackingFields
        {
            public int SerienId;
            public string Name;
            public string Bemerkung;
            public byte[] Logo;
        }
        #region Private fields
        private BackingFields bFields;
        private BackingFields clone;
        private bool _isInitializing = false;
        ISerienDataProvider _seriesProvider;
        private SerieDto _series;
        #endregion
        public SeriesEditViewModel(ISerienDataProvider provider, IEventAggregator eventAggregator):base(eventAggregator)
        {
            _seriesProvider = provider;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
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

      
        public int SeriesId
        {
            get { return bFields.SerienId; }
            set { SetProperty(ref bFields.SerienId, value); }
        }

        /// <summary>
        /// Gets or sets the Logo.
        /// </summary>
        /// <value>
        /// The Logo.
        /// </value>
        public byte[] Logo
        {
            get { return bFields.Logo; }
            set { SetProperty(ref bFields.Logo, value); }
        }

        #endregion
        #region Overrides
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (!_isInitializing)
                SaveSeries();
            return ok;
        }

        #endregion
        private async void LoadSeries(int seriesId)
        {
            _isInitializing = true;
            try
            {
                _series = await _seriesProvider.LoadSeriesById(seriesId);
                InitProperties();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex);
                ShowException(ex);
            }
            _isInitializing = false;
        }
        private void InitProperties()
        {
            SeriesId = _series.SerienId;
            Name = _series.Name;
            Bemerkung = _series.Bemerkung;
            Logo = _series.Logo;
            clone = bFields;
        }
        private void SaveSeries()
        {
            if (_series != null)
            {
                _series.SerienId = SeriesId;
                _series.Name = Name;
                _series.Bemerkung = Bemerkung;
                _series.Logo = Logo;
                try
                {
                    _seriesProvider.SaveSerie(_series);
                    EventAggregator.GetEvent<SeriesChangedEvent>().Publish(new SeriesChangedEventArgs { Series = _series, ChangedType = ChangedType.Update });
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                    ShowException(ex);
                }
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var series = navigationContext.Parameters.FirstOrDefault();
            int seriesID = (int)series.Value;
            LoadSeries(seriesID);
        }
    }
}
