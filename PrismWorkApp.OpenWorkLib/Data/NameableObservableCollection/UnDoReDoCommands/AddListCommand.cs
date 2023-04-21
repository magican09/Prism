using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddListCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand where TEntity : IEntityObject
    {
        private NameableObservableCollection<TEntity> _Collection;
        private IEnumerable<TEntity> _List;
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
            foreach (TEntity item in _List)
            {
                ChangedObjects.Add(item);
                item.ChangesJornal.Add(this);
                if (UnDoReDo_System.Contains(item))
                    item.State = EntityState.Modified;
                else
                    item.State = EntityState.Added;
                _ItemStateDict.Add(item, item.State);
            }
            _CollectionState = _Collection.State;
            _Collection.State = EntityState.Modified; 
            _OwnerState = _Collection.Owner.State;

            _Collection.ChangesJornal.Add(this);
            ChangedObjects.Add(_Collection);
            _Collection.JornalingOn();

        }
        public void UnExecute()
        {
            _Collection.JornalingOff();
            foreach (TEntity item in _List)
            {
                ChangedObjects.Remove(item);
                item.State = _ItemStateDict[item];
                item.ChangesJornal.Remove(this);
            }
            _Collection.State = _CollectionState;
            _Collection.Owner.State = _OwnerState;
            
            _Collection.ChangesJornal.Remove(this);
            ChangedObjects.Remove(_Collection);
          
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
                if (UnDoReDo_System.Contains(item))
                    item.State = EntityState.Modified;
                else
                    item.State = EntityState.Added;
                _ItemStateDict.Add(item, item.State);
            }
            _CollectionState = _Collection.State;
            _Collection.State = EntityState.Modified;
             _OwnerState = _Collection.Owner.State;
            _Collection.ChangesJornal.Add(this);
            ChangedObjects.Add(_Collection);
            _Collection.JornalingOn();
        }
    }
}
