using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddItemCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand where TEntity : IEntityObject
    {
        private NameableObservableCollection<TEntity> _Collection;
        private TEntity _Item;
        private EntityState _CollectionState;
        private EntityState _ItemState;
        private EntityState _OwnerState;

        public string Name { get; set; } = "Коллекция очищена";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _Collection.JornalingOff();

              _ItemState = _Item.State;

            if (UnDoReDo_System.Contains(_Item))
                _Item.State = EntityState.Modified;
            else
                _Item.State = EntityState.Added;

            _CollectionState = _Collection.State;
            _OwnerState = _Collection.Owner.State;
            _Collection.State = EntityState.Modified;
            _Collection.Owner.State = EntityState.Modified;
           
            _Collection.Add(_Item);
            _Item.ChangesJornal.Add(this);
            _Collection.ChangesJornal.Add(this);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);

            _Collection.JornalingOn();
        }
        public void UnExecute()
        {
            _Collection.JornalingOff();
          
               _Item.State = _ItemState;
            _Collection.State = _CollectionState;
            _Collection.Owner.State = _OwnerState;

            _Collection.Remove(_Item);
            _Item.ChangesJornal.Remove(this);
            _Collection.ChangesJornal.Remove(this);
            ChangedObjects.Remove(_Item);
            ChangedObjects.Remove(_Collection);

            _Collection.JornalingOn();
        }
        public AddItemCommand(TEntity item, NameableObservableCollection<TEntity> collection)
        {
            _Item = item;
            _Collection = collection;
            UnDoReDo_System = collection.UnDoReDoSystem;

            _Collection.JornalingOff();
              _ItemState = _Item.State;
            _CollectionState = _Collection.State;
            _OwnerState = _Collection.Owner.State;

            if (UnDoReDo_System.Contains(_Item))
                _Item.State = EntityState.Modified;
            else
                _Item.State = EntityState.Added;
            _Collection.State = EntityState.Modified;
            _Collection.Owner.State = EntityState.Modified;

            _Collection.Add(_Item);
            _Item.ChangesJornal.Add(this);
            _Collection.ChangesJornal.Add(this);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);
            _Collection.JornalingOn();
        }
    }
}
