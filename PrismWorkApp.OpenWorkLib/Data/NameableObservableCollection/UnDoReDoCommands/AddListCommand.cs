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
                ChangedObjects.Add(item);
                item.ChangesJornal.Add(this);
            }
            ChangedObjects.Add(_Collection);
            _Collection.Owner.ChangesJornal.Add(this);
            _Collection.JornalingOn();

        }
        public void UnExecute()
        {
            _Collection.JornalingOff();
            foreach (TEntity item in _List)
            {
                ChangedObjects.Remove(item);
                item.ChangesJornal.Remove(this);
            }
            ChangedObjects.Remove(_Collection);
            _Collection.Owner.ChangesJornal.Remove(this);
            _Collection.JornalingOn();
        }
        public AddListCommand(IEnumerable<TEntity> list, NameableObservableCollection<TEntity> collection)
        {
            _List = list;
            _Collection = collection;
            UnDoReDo_System = collection.UnDoReDoSystem;

            _Collection.JornalingOff();
            foreach (TEntity item in _List)
            {
                ChangedObjects.Add(item);
                item.ChangesJornal.Add(this);
            }
             ChangedObjects.Add(_Collection);
           _Collection.Owner.ChangesJornal.Add(this);
            _Collection.JornalingOn();
        }
    }
}
