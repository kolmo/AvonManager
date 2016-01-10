using AvonManager.Interfaces.Criteria;

namespace AvonManager.Data.SearchCriteria
{
    public class ArticleSearchCriteria : SearchCriteriaBase, IArticleSearchCriteria
    {
      
        private int[] _categories;
        /// <summary>
        /// Gets or sets the Categories.
        /// </summary>
        /// <value>
        /// The Categories.
        /// </value>
        public int[] Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        private int[] _markups;
        /// <summary>
        /// Gets or sets the Markups.
        /// </summary>
        /// <value>
        /// The Markups.
        /// </value>
        public int[] Markups
        {
            get { return _markups; }
            set { SetProperty(ref _markups, value); }
        }

        private int[] _series;
        /// <summary>
        /// Gets or sets the Series.
        /// </summary>
        /// <value>
        /// The Series.
        /// </value>
        public int[] Series
        {
            get { return _series; }
            set { SetProperty(ref _series, value); }
        }

        private bool _withoutMarkups;
        /// <summary>
        /// Gets or sets the WithoutMarkups.
        /// </summary>
        /// <value>
        /// The WithoutMarkups.
        /// </value>
        public bool WithoutMarkups
        {
            get { return _withoutMarkups; }
            set { SetProperty(ref _withoutMarkups, value); }
        }

        private string _fulltext;
        /// <summary>
        /// Gets or sets the Fulltext.
        /// </summary>
        /// <value>
        /// The Fulltext.
        /// </value>
        public string FullText
        {
            get { return _fulltext; }
            set { SetProperty(ref _fulltext, value); }
        }

        private bool _withoutSeries;
        /// <summary>
        /// Gets or sets the WithoutSeries.
        /// </summary>
        /// <value>
        /// The WithoutSeries.
        /// </value>
        public bool WithoutSeries
        {
            get { return _withoutSeries; }
            set { SetProperty(ref _withoutSeries, value); }
        }

        private bool _withoutCategory;
        /// <summary>
        /// Gets or sets the WithoutCategory.
        /// </summary>
        /// <value>
        /// The WithoutCategory.
        /// </value>
        public bool WithoutCategory
        {
            get { return _withoutCategory; }
            set { SetProperty(ref _withoutCategory, value); }
        }
        public override void Reset()
        {
            base.Reset();
            FullText = null;
            WithoutCategory = false;
            WithoutMarkups = false;
            WithoutSeries = false;
            Markups = null;
            Series = null;
            Categories = null;
        }
    }
}
