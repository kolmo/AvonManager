using AvonManager.BusinessObjects;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvonManager.KundenHefte.Presentation
{
    public class HeftKundenViewModel : BindableBase
    {
        KundeDto _kunde;
        public HeftKundenViewModel(KundeDto kunde)
        {
            _kunde = kunde;
        }
        private bool _received;
        /// <summary>
        /// Gets or sets the Received.
        /// </summary>
        /// <value>
        /// The Received.
        /// </value>
        public bool Received
        {
            get { return _received; }
            set { SetProperty(ref _received, value); }
        }

        /// <summary>
        /// Gets or sets the Customer.
        /// </summary>
        /// <value>
        /// The Customer.
        /// </value>
        public string Customer
        {
            get { return _kunde.Nachname; }
        }

        private DateTime? _receivedAt;
        /// <summary>
        /// Gets or sets the ReceivedAt.
        /// </summary>
        /// <value>
        /// The ReceivedAt.
        /// </value>
        public DateTime? ReceivedAt
        {
            get { return _receivedAt; }
            set { SetProperty(ref _receivedAt, value); }
        }

        private bool _hasOrdered;
        /// <summary>
        /// Gets or sets the HasOrdered.
        /// </summary>
        /// <value>
        /// The HasOrdered.
        /// </value>
        public bool HasOrdered
        {
            get { return _hasOrdered; }
            set { SetProperty(ref _hasOrdered, value); }
        }

    }
}
