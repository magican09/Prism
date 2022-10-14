using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public  interface IValidateable
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
