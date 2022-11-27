using System;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public class AddToCollectionCommand<TColloction, TEntity> : IUnDoRedoCommand
       where TColloction : ICollection<TEntity>
    {
        private TColloction _Collection;
        private TEntity _AddObject;
        public string Name { get; set; }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
        public virtual void Execute(object parameter = null)
        {
            if (parameter != null)
                _AddObject = (TEntity)parameter;
            _Collection.Add(_AddObject);
        }

        public virtual void UnExecute()
        {
            _Collection.Remove(_AddObject);
        }

        public AddToCollectionCommand(TColloction colloction, TEntity added_object)
        {
            _Collection = colloction;
            _AddObject = added_object;
            _Collection.Add(_AddObject);
        }

    }
}
