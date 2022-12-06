using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public interface INotifyCommand:ICommand
    {
       string Name { get; set; }
       
    }
}