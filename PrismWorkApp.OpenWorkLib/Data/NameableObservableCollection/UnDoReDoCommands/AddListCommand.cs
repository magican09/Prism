using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddListCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand where TEntity : IEntityObject
    {
        private NameableObservableCollection<TEntity> _Collection;
        private IEnumerable<TEntity> _List;

        public string Name { get; set; } = "Элемент добавлен";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _Collection.JornalingOff();
            foreach (TEntity item in _List)
            {
                if (_Collection.Owner != null)
                    item.Parents.Add(_Collection.Owner);
                else
                    item.Parents.Add(_Collection);
                _Collection.Add(item);
                ChangedObjects.Add(item);
                item.ChangesJornal.Add(this);

            }
            ChangedObjects.Add(_Collection);
            _Collection.ChangesJornal.Add(this);
            if (_Collection.Owner != null) _Collection.Owner.ChangesJornal.Add(this);
            _Collection.JornalingOn();

        }
        public void UnExecute()
        {
            _Collection.JornalingOff();
            foreach (TEntity item in _List)
            {
                if (_Collection.Owner != null)
                    item.Parents.Remove(_Collection.Owner);
                else
                    item.Parents.Remove(_Collection);
                _Collection.Remove(item);
                ChangedObjects.Remove(item);
                item.ChangesJornal.Remove(this);

            }
            ChangedObjects.Remove(_Collection);
            _Collection.ChangesJornal.Remove(this);
            if (_Collection.Owner != null) _Collection.Owner.ChangesJornal.Remove(this);
            _Collection.JornalingOn();
        }
        public AddListCommand(IEnumerable<TEntity> list, NameableObservableCollection<TEntity> collection)
        {
            _List = list;
            _Collection = collection;

            _Collection.JornalingOff();
            foreach (TEntity item in _List)
            {
                if (_Collection.Owner != null)
                    item.Parents.Add(_Collection.Owner);
                else
                    item.Parents.Add(_Collection);
                _Collection.Add(item);
                ChangedObjects.Add(item);
                item.ChangesJornal.Add(this);
                
            }
            ChangedObjects.Add(_Collection);
            _Collection.ChangesJornal.Add(this);
            if (_Collection.Owner != null) _Collection.Owner.ChangesJornal.Add(this);
            _Collection.JornalingOn();
        }
    }
}
