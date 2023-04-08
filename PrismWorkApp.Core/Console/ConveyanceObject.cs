namespace PrismWorkApp.Core
{
    public class ConveyanceObject
    {
        public bool EditMode { get; set; }
        public object Object { get; set; }
        public ConveyanceObject(object obj, bool edit_mode)
        {
            EditMode = edit_mode;
            Object = obj;
        }
        public ConveyanceObject()
        {

        }

    }
    public class ConveyanceObjectModes
    {
        public static class EditMode
        {
            public const bool FOR_EDIT = true;
            public const bool NEW = false;
        }

    }
}
