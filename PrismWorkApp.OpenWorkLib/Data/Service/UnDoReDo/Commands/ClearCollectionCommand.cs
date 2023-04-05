using System;
using System.Collections;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{

    public class ClearCollectionCommand<TCollection, TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand
        where TCollection : IList, IEnumerable
    {
        private TCollection _Collection;

        private Stack<TEntity> _RemovedEntities = new Stack<TEntity>();
        public string Name { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            foreach (TEntity ent in _Collection)
            {
                _RemovedEntities.Push(ent);
            }
            foreach (TEntity ent in _RemovedEntities)
            {
                _Collection.Remove(ent);
            }
        }

        public void UnExecute()
        {

            while (_RemovedEntities.Count > 0)
                _Collection.Add(_RemovedEntities.Pop());


        }
        public ClearCollectionCommand(TCollection collection)
        {
            _Collection = collection;
            foreach (TEntity ent in _Collection)
            {
                _RemovedEntities.Push(ent);
            }
            foreach (TEntity ent in _RemovedEntities)
            {
                _Collection.Remove(ent);
            }

        }
    }
}
