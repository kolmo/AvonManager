using Prism.Commands;
using System;

namespace AvonManager.Common.Base
{
    public class ListEntryBaseViewModel<T> : FilterEntryBase
    {
        public ListEntryBaseViewModel() { }
        public ListEntryBaseViewModel(Action<T> editAction, Action<T> deleteAction)
        {
            if (editAction== null || deleteAction== null)
            {
                throw new ArgumentNullException("editAction|deleteAction");
            }
            EditCommand = new DelegateCommand<T>(editAction);
            DeleteCommand = new DelegateCommand<T>(deleteAction);
        }
        public DelegateCommand<T> EditCommand { get; private set; }
        public DelegateCommand<T> DeleteCommand { get; private set; }
    }
}
