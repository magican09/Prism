using Prism;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
   public abstract class NotifyCommandBase 
    {
        
        protected abstract bool CanExecute(object parameter);
        //
      

       
    }
}
