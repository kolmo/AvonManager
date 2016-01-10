namespace AvonManager.Data.SearchCriteria
{
    public class BrochureSearchCriteria : SearchCriteriaBase, AvonManager.Interfaces.Criteria.IBrochureSearchCriteria
    {

        private string _fullText;
        /// <summary>
        /// Gets or sets the FullText.
        /// </summary>
        /// <value>
        /// The FullText.
        /// </value>
        public string FullText
        {
            get { return _fullText; }
            set { SetProperty(ref _fullText, value); }
        }


        private int[] _years;
        /// <summary>
        /// Gets or sets the Years.
        /// </summary>
        /// <value>
        /// The Years.
        /// </value>
        public int[] Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
        }


        public override void Reset()
        {
            FullText = null;
            Years = null;
        }
    }
}
