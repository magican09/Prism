namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IHierarchicable<TParentType, TChildrenType>
    {
        public TParentType Parent { get; set; }
        public TChildrenType Children { get; set; }
    }
}
