using Prism;
using System;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public interface INotifyCommand : ICommand, IActiveAware
    {
        string Name { get; set; }
        Uri ImageUri { get; set; }
        bool MonitorCommandActivity { get; set; }
    }
}