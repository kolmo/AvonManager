using AvonManager.BusinessObjects;
using AvonManager.Common.Helpers;
using AvonManager.Interfaces;
using Prism.Mvvm;
using System;
using System.Runtime.CompilerServices;

namespace AvonManager.ArtikelModule.Views
{
    public class MarkingEditViewModel : BindableBase
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
        IMarkierungenDataProvider _markingProvider;
        private MarkierungDto _marking;
        #endregion
        public MarkingEditViewModel(MarkierungDto marking, IMarkierungenDataProvider provider)
        {
            _markingProvider = provider;
            _marking = marking;
            InitProperties();
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
                SaveMarking();
            return ok;
        }

        #endregion
      
        private void InitProperties()
        {
          bFields.MarkierungId  = _marking.MarkierungId;
           bFields.Titel = _marking.Titel;
           bFields.Bemerkung = _marking.Bemerkung;
           bFields.FarbeARGB = _marking.FarbeARGB;
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
