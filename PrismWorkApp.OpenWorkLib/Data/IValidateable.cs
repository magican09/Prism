using System;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IValidateable
    {
          event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
