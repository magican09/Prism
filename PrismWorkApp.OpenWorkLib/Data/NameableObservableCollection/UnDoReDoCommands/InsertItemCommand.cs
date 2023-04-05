using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class InsertItemCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand where TEntity: IEntityObject
    {
        private NameableObservableCollection<TEntity> _Collection;
        private TEntity _Item;
        
        public string Name { get; set; } = "Элемент добавлен";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _Collection.JornalingOff();
            _Item.Parents.Add(_Collection.Owner); 
            _Collection.Add(_Item);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);
            _Collection.JornalingOn();

        }
        public void UnExecute()
        {
            _Collection.JornalingOff();
            _Item.Parents.Remove(_Collection.Owner); 
            _Collection.Remove(_Item);
            ChangedObjects.Remove(_Item);
            ChangedObjects.Remove(_Collection);
            _Collection.JornalingOn();
        }
        public InsertItemCommand(int index,TEntity  item, NameableObservableCollection<TEntity> collection)
        {
            _Item = item;
            _Collection = collection;

            _Collection.JornalingOff();
            _Item.Parents.Add(_Collection.Owner);
            _Collection.Add(_Item);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);
            _Collection.JornalingOn();
        }
    }
}
