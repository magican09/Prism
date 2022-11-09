using Prism;
using Prism.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public interface IApplicationCommands
    {
        NotifyCompositeCommand SaveAllCommand { get; }
       NotifyCompositeCommand UnDoLeftCommand { get; }
        NotifyCompositeCommand UnDoRightCommand { get; }
    }
    public class ApplicationCommands : IApplicationCommands
    {
        private NotifyCompositeCommand _saveAllCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand SaveAllCommand
        {
            get { return _saveAllCommand; }
        }
        private NotifyCompositeCommand _unDoLeftCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand UnDoLeftCommand
        {
            get { return _unDoLeftCommand; }
        }
        private NotifyCompositeCommand _unDoRightCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand UnDoRightCommand
        {
            get { return _unDoRightCommand; }
        }
        // public CompositeCommand LoadProjectFromExcell { get; } = new CompositeCommand();
    }
    
   
   
}