using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class ClearCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand where TEntity : IEntityObject
    {
        private NameableObservableCollection<TEntity> _Collection;
        private IList<TEntity> _RemovedItemsCollection;
        private EntityState _CollectionState;
        private Dictionary<TEntity, EntityState> _ItemStateDict = new Dictionary<TEntity, EntityState>();
        private EntityState _OwnerState;

        public string Name { get; set; } = "Элемент добавлен";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _Collection.JornalingOff();
            _RemovedItemsCollection = new List<TEntity>(_Collection);
            _Collection.Clear();
            foreach (TEntity item in _RemovedItemsCollection)
            {
                if (UnDoReDo_System.Contains(item))
                    item.State = EntityState.Modified;
                else
                    item.State = EntityState.Removed;

                ChangedObjects.Add(item);
                item.ChangesJornal.Add(this);
                _ItemStateDict.Add(item, item.State);
            }
            _OwnerState = _Collection.Owner.State;
            _Collection.Owner.State = EntityState.Modified;
            _CollectionState = _Collection.State;
            _Collection.State = EntityState.Modified;
           _Collection.ChangesJornal.Add(this);
            ChangedObjects.Add(_Collection);
            _Collection.JornalingOn();

        }
        public void UnExecute()
        {
            _Collection.JornalingOff();

            foreach (TEntity item in _RemovedItemsCollection)
            {
                item.ChangesJornal.Add(this);
                item.State = _ItemStateDict[item];

                _Collection.Add(item);
                ChangedObjects.Remove(item);
            }

            _Collection.State = _CollectionState;
            _Collection.Owner.State = _OwnerState;

            _Collection.ChangesJornal.Remove(this);
            ChangedObjects.Remove(_Collection);
            _Collection.JornalingOn();
        }
        public ClearCommand(NameableObservableCollection<TEntity> collection)
        {

            _Collection = collection;
            UnDoReDo_System = collection.UnDoReDoSystem;

            _Collection.JornalingOff();
            _RemovedItemsCollection = new List<TEntity>(_Collection);
            _Collection.Clear();
            _OwnerState = _Collection.Owner.State;

            foreach (TEntity item in _RemovedItemsCollection)
            {
                if (UnDoReDo_System.Contains(item))
                    item.State = EntityState.Modified;
                else
                    item.State = EntityState.Removed;

                ChangedObjects.Add(item);
                item.ChangesJornal.Add(this);
                _ItemStateDict.Add(item, item.State);
            }
            _Collection.Owner.State = EntityState.Modified;

            _CollectionState = _Collection.State;
            _Collection.State = EntityState.Modified;
            
            _Collection.ChangesJornal.Add(this);
            ChangedObjects.Add(_Collection);
            _Collection.JornalingOn();
        }
    }
}
