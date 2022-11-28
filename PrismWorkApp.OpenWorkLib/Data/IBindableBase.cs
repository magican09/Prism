namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IBindableBase
    {
        //    public bool IsUnDoReDoSystemIsEmpty(Guid currentContextId);
        //  public void SetCopy<TSourse>(object pointer, Func<TSourse, bool> predicate) where TSourse : IEntityObject;
        public bool IsVisible { get; set; }
    }
}
