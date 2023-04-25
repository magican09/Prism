using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class RemoveItemCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand where TEntity : IEntityObject
    {
        private NameableObservableCollection<TEntity> _Collection;
        private TEntity _Item;
        private int _Index;
        private EntityState _CollectionState;
        private EntityState _ItemState;
        private EntityState _OwnerState;
        public string Name { get; set; } = "Элемент удален";

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
                _Item.State = EntityState.Removed;
            //  _CollectionState = _Collection.State;
            //_Collection.State = EntityState.Modified;
            _OwnerState = _Collection.Owner.State;
            if (UnDoReDo_System.Contains(_Collection.Owner))
                _Collection.Owner.State = EntityState.Modified;

            _Collection.Remove(_Item);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);
            _Item.ChangesJornal.Add(this);
            _Collection.ChangesJornal.Add(this);
            _Collection.JornalingOn();

        }
        public void UnExecute()
        {
            _Collection.JornalingOff();
           
            //_Collection.State = _CollectionState;
            _Collection.Owner.State = _OwnerState;
            _Item.State = _ItemState;

            _Collection.Insert(_Index, _Item);
            ChangedObjects.Remove(_Item);
            ChangedObjects.Remove(_Collection);
            _Item.ChangesJornal.Remove(this);
            _Collection.ChangesJornal.Remove(this);
            _Collection.JornalingOn();
        }
        public RemoveItemCommand(TEntity item, int index, NameableObservableCollection<TEntity> collection)
        {
            _Item = item;
            _Index = index;
            _Collection = collection;
            UnDoReDo_System = collection.UnDoReDoSystem;

            _Collection.JornalingOff();
            _ItemState = _Item.State;
            if (UnDoReDo_System.Contains(_Item))
                _Item.State = EntityState.Modified;
            else
                _Item.State = EntityState.Removed;

            //_CollectionState = _Collection.State;
            //_Collection.State = EntityState.Modified;
             _OwnerState = _Collection.Owner.State;
            if (UnDoReDo_System.Contains(_Collection.Owner))
                _Collection.Owner.State = EntityState.Modified;

            _Collection.Remove(_Item);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);
            _Item.ChangesJornal.Add(this);
            _Collection.ChangesJornal.Add(this);
            _Collection.JornalingOn();
        }
    }
}
