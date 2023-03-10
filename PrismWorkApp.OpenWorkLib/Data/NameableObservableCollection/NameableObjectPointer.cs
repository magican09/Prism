namespace PrismWorkApp.OpenWorkLib.Data
{
    public class NameableObjectPointer : INameable, ICodeable
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Annotation { get; set; }
        public IEntityObject ObjectPointer;
    }
}
