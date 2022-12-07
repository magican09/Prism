﻿using System.Windows.Input;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IUnDoRedoCommand : ICommand
    {

        public string Name { get; set; }
        void Execute(object parameter = null);
        public void UnExecute();
    }
}
