namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IHierarchicable<TParentType, TChildrenType>
    {
          TParentType Parent { get; set; }
          TChildrenType Children { get; set; }
    }
}
