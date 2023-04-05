using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{ 
  public abstract class UnDoRedoCommandBase
    {
      public ObservableCollection<IJornalable>  ChangedObjects {get;set;} = new ObservableCollection<IJornalable>();
    }
}
