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
        public CompositeCommand SaveAllCommand { get; } = new CompositeCommand();
        public CompositeCommand LoadProjectFromExcell { get; } = new CompositeCommand();
    }
}
