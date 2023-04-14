using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class InsertItemCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand where TEntity : IEntityObject
    {
        private NameableObservableCollection<TEntity> _Collection;
        private TEntity _Item;
        private int _Index;

        public string Name { get; set; } = "Элемент  вставлен";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _Collection.JornalingOff();
            _Collection.Insert(_Index, _Item);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);
            _Item.ChangesJornal.Add(this);
            _Collection.ChangesJornal.Add(this);
           // if (_Collection.Owner != null && _Collection.Owner.Id != _Collection.Id) _Collection.Owner.ChangesJornal.Add(this);
            _Collection.JornalingOn();

        }
        public void UnExecute()
        {
            _Collection.JornalingOff();
            _Collection.Remove(_Item);
            ChangedObjects.Remove(_Item);
            ChangedObjects.Remove(_Collection);
            _Item.ChangesJornal.Remove(this);
            _Collection.ChangesJornal.Add(this);
           // if (_Collection.Owner != null && _Collection.Owner.Id != _Collection.Id) _Collection.Owner.ChangesJornal.Remove(this);
            _Collection.JornalingOn();
        }
        public InsertItemCommand(int index, TEntity item, NameableObservableCollection<TEntity> collection)
        {
            _Item = item;
            _Index = index;
            _Collection = collection;
            UnDoReDo_System = collection.UnDoReDoSystem;

            _Collection.JornalingOff();
            _Collection.Insert(_Index,_Item);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);
            _Item.ChangesJornal.Add(this);
            _Collection.ChangesJornal.Add(this);
           // if (_Collection.Owner != null&& _Collection.Owner.Id!= _Collection.Id) _Collection.Owner.ChangesJornal.Add(this);
            _Collection.JornalingOn();
        }
    }
}
