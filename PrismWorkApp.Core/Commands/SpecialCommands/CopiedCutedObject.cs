using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core.Commands
{
    public class CopiedCutedObject<T>
    {
        public object FromObject { get; set; }
        public T Element  {get;set;}
        public CopyCutPaste ActionType { get; set; }
        public CopiedCutedObject(object from_object,T element, CopyCutPaste action_type)
        {
            FromObject = from_object;
            Element = element;
            ActionType = action_type;
        }
        public CopiedCutedObject()
        {

        }
    }
    public enum CopyCutPaste
    {
        COPIED,
        CUTED,
        PASTED
    }
}
