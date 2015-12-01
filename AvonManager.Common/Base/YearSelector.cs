using AvonManager.Common.Base;

namespace AvonManager.Common
{
    public class YearSelector : FilterEntryBase
    {
        private int _year;
        /// <summary>
        /// Gets or sets the Year.
        /// </summary>
        /// <value>
        /// The Year.
        /// </value>
        public int Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }
    }
}
