using System;

namespace AvonManager.Data
{
    class CustomerSearchCriteria : SearchCriteriaBase, AvonManager.Interfaces.ICustomerSearchCriteria
    {

        private int? _brochureId;
        /// <summary>
        /// Gets or sets the BrochureId.
        /// </summary>
        /// <value>
        /// The BrochureId.
        /// </value>
        public int? BrochureId
        {
            get { return _brochureId; }
            set { SetProperty(ref _brochureId, value); }
        }


        private bool? _getsBrochure;
        /// <summary>
        /// Gets or sets the GetsBrochure.
        /// </summary>
        /// <value>
        /// The GetsBrochure.
        /// </value>
        public bool? GetsBrochure
        {
            get { return _getsBrochure; }
            set { SetProperty(ref _getsBrochure, value); }
        }

        private bool? _hasOrders;
        /// <summary>
        /// Gets or sets the HasOrders.
        /// </summary>
        /// <value>
        /// The HasOrders.
        /// </value>
        public bool? HasOrders
        {
            get { return _hasOrders; }
            set { SetProperty(ref _hasOrders, value); }
        }

        private string _initial;
        /// <summary>
        /// Gets or sets the Initial.
        /// </summary>
        /// <value>
        /// The Initial.
        /// </value>
        public string Initial
        {
            get { return _initial; }
            set { SetProperty(ref _initial, value); }
        }

        private string _searchString;
        /// <summary>
        /// Gets or sets the SearchString.
        /// </summary>
        /// <value>
        /// The SearchString.
        /// </value>
        public string SearchString
        {
            get { return _searchString; }
            set { SetProperty(ref _searchString, value); }
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

        private bool? _inActiveCustomersOnly;
        /// <summary>
        /// Gets or sets the InActiveCustomersOnly.
        /// </summary>
        /// <value>
        /// The InActiveCustomersOnly.
        /// </value>
        public bool? InActiveCustomersOnly
        {
            get { return _inActiveCustomersOnly; }
            set { SetProperty(ref _inActiveCustomersOnly, value); }
        }
        public override void Reset()
        {
            BrochureId = null;
            GetsBrochure = null;
            Initial = null;
            SearchString = null;
            HasOrders = null;
            InActiveCustomersOnly = null;
            ActiveCustomersOnly = false;
        }

    }
}
