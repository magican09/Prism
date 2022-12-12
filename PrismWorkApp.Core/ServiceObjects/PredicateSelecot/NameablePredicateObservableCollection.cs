using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Core
{
    public class NameablePredicateObservableCollection<TSourse, TOut> : ObservableCollection<NameablePredicate<TSourse, TOut>>
        where TOut: IEntityObject
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class NameablePredicateObservableCollection : ObservableCollection<NameablePredicate>
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
