using AvonManager.BusinessObjects;
using AvonManager.Common.Events;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Runtime.CompilerServices;

namespace AvonManager.ArtikelModule.Views
{
    public class SeriesEditViewModel : BindableBase
    {
        private struct BackingFields
        {
            public int SerienId;
            public string Name;
            public string Bemerkung;
        }
        #region Private fields
        private BackingFields bFields;
        private BackingFields clone;
        ISerienDataProvider _seriesProvider;
        private SerieDto _series;
        private IEventAggregator _eventAggregator;
        #endregion
        public SeriesEditViewModel(SerieDto series, ISerienDataProvider provider, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _seriesProvider = provider;
            _series = series;
            InitProperties();
        }

        #region Properties
        public SerieDto DTO { get { return _series; } }
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

        private int _articleCount;
        /// <summary>
        /// Gets or sets the ArticleCount.
        /// </summary>
        /// <value>
        /// The ArticleCount.
        /// </value>
        public int ArticleCount
        {
            get { return _articleCount; }
            set
            {
                if (_articleCount != value)
                {
                    _articleCount = value;
                    RaisePropertyChanged(nameof(ArticleCount));
                }
            }
        }
        #endregion
        #region Overrides
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            SaveSeries();
            _eventAggregator.GetEvent<SeriesChangedEvent>().Publish(new SeriesChangedEventArgs { Series = _series, ChangedType = ChangedType.Update });
            return ok;
        }

        #endregion
        private void InitProperties()
        {
            bFields.SerienId = _series.SerienId;
            bFields.Name = _series.Name;
            bFields.Bemerkung = _series.Bemerkung;
            clone = bFields;
        }
        private void SaveSeries()
        {
            if (_series != null)
            {
                _series.SerienId = SeriesId;
                _series.Name = Name;
                _series.Bemerkung = Bemerkung;
                try
                {
                    _seriesProvider.SaveSerie(_series);
                    clone = bFields;
                    
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex);
                }
            }
        }
    }
}
