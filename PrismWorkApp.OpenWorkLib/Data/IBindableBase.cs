using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IBindableBase: ICopingEnableable
    {
    //    public bool IsPropertiesChangeJornalIsEmpty(Guid currentContextId);
      //  public void SetCopy<TSourse>(object pointer, Func<TSourse, bool> predicate) where TSourse : IEntityObject;
        public bool IsVisible { get; set;}
    }
}
