﻿using Prism;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public interface INotifyCommand : ICommand, IActiveAware
    {
        string Name { get; set; }

    }
}