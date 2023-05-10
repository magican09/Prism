using System;

namespace PrismWorkApp.Core
{
    public interface IEventNotifyable
    {
          event EventHandler EventNotify;
    }
}
