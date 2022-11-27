using System;

namespace PrismWorkApp.Core
{
    public interface IEventNotifyable
    {
        public event EventHandler EventNotify;
    }
}
