using System;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IValidateable
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
