﻿using System.Collections.Generic;
using System.Linq;
namespace AvonManager.Data
{
    public partial class Serien
    {
        #region Fields
        private IList<int> _idsInclChildren = null;
        private bool _isSelected4Filter;
        private const string INITIALBREADCRUMB = "Alle Serien";
        private string _breadcrumb = INITIALBREADCRUMB;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the IsSelected.
        /// </summary>
        /// <value>
        /// The IsSelected.
        /// </value>
        public bool IsSelected4Filter
        {
            get { return _isSelected4Filter; }
            set
            {
                if (_isSelected4Filter != value)
                {
                    _isSelected4Filter = value;
                }
            }
        }

        public IList<int> IdsInclChildren { get { return _idsInclChildren; } }

        /// <summary>
        /// Gets or sets the Breadcrumb.
        /// </summary>
        /// <value>
        /// The Breadcrump.
        /// </value>
        public string Breadcrumb
        {
            get { return _breadcrumb; }
            set
            {
                if (_breadcrumb != value)
                {
                    _breadcrumb = value;
                }
            }
        }

        #endregion Properties

        #region Public methods
        public void CreateBreadcrumb()
        {
            if (Breadcrumb == INITIALBREADCRUMB)
            {
                Breadcrumb = this.Name;
                GetParent(this);               
            }
        }
        public void AddKindId(int id)
        {
            IdsInclChildren.Add(id);
        }
        public void AddParentName(string name)
        {
            Breadcrumb = name + " > " + Breadcrumb;
        }
        #endregion Public methods

        #region Private Methods
        private void GetParent(Serien parent)
        {
            //Serien p = this.EntitySet.Cast<Serien>().FirstOrDefault(x => x.SerienId == parent.Parent);
            //if (p != null)
            //{
            //    AddParentName(p.Name);
            //    GetParent(p);
            //}
        }

        #endregion
    }
}
