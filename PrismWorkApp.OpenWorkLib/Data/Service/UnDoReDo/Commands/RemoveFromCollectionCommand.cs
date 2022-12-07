using System;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class RemoveFromCollectionCommand<TColloction, TEntity> : IUnDoRedoCommand
        where TColloction : ICollection<TEntity>
    {
        private TColloction _Collection;
        private TEntity _RemovedObject;
        public string Name { get; set; }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
        public virtual void Execute(object parameter = null)
        {
            if (parameter != null)
                _RemovedObject = (TEntity)parameter;
            _Collection.Remove((TEntity)_RemovedObject);

        }
        public virtual void UnExecute()
        {
            _Collection.Add(_RemovedObject);
        }
        public RemoveFromCollectionCommand(TColloction colloction, TEntity removed_object)
        {
            _Collection = colloction;
            _RemovedObject = removed_object;
            _Collection.Remove(_RemovedObject);
        }

    }
}
