
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace AvonManager.Common.Base
{
    public class DeleteEntityConfirmation<T> : Confirmation
    {
        public DeleteEntityConfirmation(T entity)
        {
            EntityToDelete = entity;
        }
        public T EntityToDelete{ get;}
    }
}
