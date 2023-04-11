using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IUnDoReDoCompositeCommand
    {
        string Name { get; set; }

        event EventHandler CanExecuteChanged;

        void Add(IUnDoRedoCommand command);
        bool CanExecute(object parameter);
        void Execute(object parameter = null);
        void UnExecute();
    }
}