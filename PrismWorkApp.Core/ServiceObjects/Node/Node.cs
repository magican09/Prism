using Prism.Mvvm;
using System.Reflection;

namespace PrismWorkApp.Core
{
    public class Node : BindableBase, INode
    {

        private INodes _nodes = new Nodes();
        public INodes Nodes
        {
            get { return _nodes; }
            set { SetProperty(ref _nodes, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private NodeType _nodeType;
        public NodeType PropertyName
        {
            get { return _nodeType; }
            set { SetProperty(ref _nodeType, value); }
        }
        private object _value;
        public object Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
        private INode _parentNode;
        public INode ParentNode
        {
            get { return _parentNode; }
            set { SetProperty(ref _parentNode, value); }
        }
        public Node()
        {

        }
        public Node(string name)
        {
            Name = name;
        }
        private PropertyInfo _propertyInfo;
        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
            set { SetProperty(ref _propertyInfo, value); }
        }

    }

    public enum NodeType
    {
        ROOT_NODE = 0,
        TERMINAL_NODE,
        COMBINE_NODE

    }
}
