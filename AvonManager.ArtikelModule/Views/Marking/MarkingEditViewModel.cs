using AvonManager.ArtikelModule.ViewModels;
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
    public class MarkingEditViewModel : ErrorAwareBaseViewModel, INavigationAware
    {
        private struct BackingFields
        {
            public int MarkierungId;
            public string Titel;
            public string Bemerkung;
            public int? FarbeARGB;
        }
        #region Private fields
        private BackingFields bFields;
        private BackingFields clone;
        private bool _isInitializing = false;
        IMarkierungenDataProvider _markingProvider;
        private MarkierungDto _marking;
        #endregion
        public MarkingEditViewModel(IMarkierungenDataProvider provider,
              IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _markingProvider = provider;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Titel
        {
            get { return bFields.Titel; }
            set { SetProperty(ref bFields.Titel, value); }
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


        public int MarkierungId
        {
            get { return bFields.MarkierungId; }
            set { SetProperty(ref bFields.MarkierungId, value); }
        }

        /// <summary>
        /// Gets or sets the Logo.
        /// </summary>
        /// <value>
        /// The Logo.
        /// </value>
        public int? FarbeARGB
        {
            get { return bFields.FarbeARGB; }
            set { SetProperty(ref bFields.FarbeARGB, value); }
        }

        #endregion
        #region Overrides
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool ok = base.SetProperty<T>(ref storage, value, propertyName);
            if (!_isInitializing)
                SaveMarking();
            return ok;
        }

        #endregion
        private async void LoadMarking(int markingId)
        {
            _isInitializing = true;
            try
            {
                _marking = await _markingProvider.LoadMarkingById(markingId);
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
            MarkierungId = _marking.MarkierungId;
            Titel = _marking.Titel;
            Bemerkung = _marking.Bemerkung;
            FarbeARGB = _marking.FarbeARGB;
            clone = bFields;
        }
        private void SaveMarking()
        {
            if (_marking != null)
            {
                _marking.MarkierungId = MarkierungId;
                _marking.Titel = Titel;
                _marking.Bemerkung = Bemerkung;
                _marking.FarbeARGB = FarbeARGB;
                try
                {
                    _markingProvider.SaveMarkierung(_marking);
                    EventAggregator.GetEvent<MarkingChangedEvent>().Publish(new MarkingChangedEventArgs { Marking = _marking, ChangedType = ChangedType.Update });
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
            var markingParameter = navigationContext.Parameters.FirstOrDefault();
            if (markingParameter.Value is MarkingListEntryViewModel)
            {
                int markingId = ((MarkingListEntryViewModel)markingParameter.Value).MarkierungId;
                LoadMarking(markingId);
            }
            else
            {
                _marking = new MarkierungDto();
                InitProperties();
            }       
        }
    }
}
