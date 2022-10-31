using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core.Commands
{
    public interface  IApplicationCommands 
        {
        CompositeCommand SaveAllCommand { get; }
    }
    public class ApplicationCommands : IApplicationCommands
    {
        private CompositeCommand _saveAllCommand = new CompositeCommand(true);
        public CompositeCommand SaveAllCommand
        {
            get { return _saveAllCommand; }
        }
        // public CompositeCommand LoadProjectFromExcell { get; } = new CompositeCommand();
    }
}
