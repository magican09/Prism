﻿using Prism;
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
        CompositeCommand SaveAllCommand { get; }
    }
    public class ApplicationCommands : IApplicationCommands
    {
        private CompositeCommand _saveAllCommand = new CompositeCommand();
        public CompositeCommand SaveAllCommand
        {
            get { return _saveAllCommand; }
        }

        // public CompositeCommand LoadProjectFromExcell { get; } = new CompositeCommand();
    }
    
   
   
}