using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core
{
   public  interface IEventNotifyable
    {
        public event EventHandler EventNotify;
    }
}
