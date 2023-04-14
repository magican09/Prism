using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddItemCommand<TEntity> : UnDoRedoCommandBase, IUnDoRedoCommand where TEntity : IEntityObject
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
            _Collection.Add(_Item);
            _Item.ChangesJornal.Add(this);
            _Collection.ChangesJornal.Add(this);
            ChangedObjects.Add(_Item);
            ChangedObjects.Add(_Collection);
           _Collection.JornalingOn();
        }
    }
}
