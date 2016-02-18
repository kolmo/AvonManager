namespace AvonManager.Data
{
    public class OrderSearchCriteria : SearchCriteriaBase, AvonManager.Interfaces.IOrderSearchCriteria
    {

        private string _articleNumber;
        /// <summary>
        /// Gets or sets the ArticleNumber.
        /// </summary>
        /// <value>
        /// The ArticleNumber.
        /// </value>
        public string ArticleNumber
        {
            get { return _articleNumber; }
            set { SetProperty(ref _articleNumber, value); }
        }

        private string _campaign;
        /// <summary>
        /// Gets or sets the Campaign.
        /// </summary>
        /// <value>
        /// The Campaign.
        /// </value>
        public string Campaign
        {
            get { return _campaign; }
            set { SetProperty(ref _campaign, value); }
        }

        private int[] _customerIds;
        /// <summary>
        /// Gets or sets the CustomerId.
        /// </summary>
        /// <value>
        /// The CustomerId.
        /// </value>
        public int[] CustomerIds
        {
            get { return _customerIds; }
            set { SetProperty(ref _customerIds, value); }
        }

        private bool _activeCustomersOnly;
        /// <summary>
        /// Gets or sets the ActiveCustomersOnly.
        /// </summary>
        /// <value>
        /// The ActiveCustomersOnly.
        /// </value>
        public bool ActiveCustomersOnly
        {
            get { return _activeCustomersOnly; }
            set { SetProperty(ref _activeCustomersOnly, value); }
        }

        private string _fullText;
        /// <summary>
        /// Gets or sets the Fulltext.
        /// </summary>
        /// <value>
        /// The Fulltext.
        /// </value>
        public string FullText
        {
            get { return _fullText; }
            set { SetProperty(ref _fullText, value); }
        }

        private int[] _orderYears;
        /// <summary>
        /// Gets or sets the OrderYears.
        /// </summary>
        /// <value>
        /// The OrderYears.
        /// </value>
        public int[] OrderYears
        {
            get { return _orderYears; }
            set { SetProperty(ref _orderYears, value); }
        }

        public override void Reset()
        {
            FullText = null;
            CustomerIds = null;
            ActiveCustomersOnly = true;
            Campaign = null;
            ArticleNumber = null;
            OrderYears = null;
        }
    }
}
