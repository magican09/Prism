using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class AddItemCommand<TEntity> : IUnDoRedoCommand where TEntity: IEntityObject
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
            _Collection.JornalingOn();

        }
        public void UnExecute()
        {
            _Collection.JornalingOff();
            _Collection.Remove(_Item);
            _Collection.JornalingOn();
        }
        public AddItemCommand(TEntity  item, NameableObservableCollection<TEntity> collection)
        {
            _Item = item;
            _Collection = collection;

            _Collection.JornalingOff();
            _Collection.Add(_Item);
            _Collection.JornalingOn();
        }
    }
}
