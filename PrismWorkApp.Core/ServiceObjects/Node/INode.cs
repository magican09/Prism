using System.Reflection;

namespace PrismWorkApp.Core
{
    public interface INode
    {
        string Name { get; set; }
        INodes Nodes { get; set; }
        INode ParentNode { get; set; }
        NodeType PropertyName { get; set; }
        object Value { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
}