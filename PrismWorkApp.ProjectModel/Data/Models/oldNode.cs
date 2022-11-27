using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class oldNode : Object, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private const int CURRENT_MODULE_ID = 2;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private NodeType _nodeType;
        public NodeType Type { get { return _nodeType; } set { _nodeType = value; OnPropertyChanged("Type"); } }
        private string _nodeName;
        public string NodeName
        {
            get { return _nodeName; }
            set { _nodeName = value; OnPropertyChanged("NodeName"); }
        }
        private object _value;
        public object Value { get { return _value; } set { _value = value; OnPropertyChanged("Value"); } }
        private ObservableCollection<oldNode> _nodes;
        public ObservableCollection<oldNode> Nodes { get { return _nodes; } set { _nodes = value; OnPropertyChanged("Nodes"); } }
        private oldNode _parentNode;
        public oldNode ParentNode { get { return _parentNode; } set { _parentNode = value; OnPropertyChanged("ParentNode"); } }

        public oldNode(string name = "", object value = null, oldNode parent_node = null, NodeType nodeType = NodeType.TERMINAL_NODE)
        {
            Value = value;
            NodeName = name;
            //   NodeName = ((IItem)Value)?.FullName; 
            Nodes = new ObservableCollection<oldNode>();
            ParentNode = parent_node;
        }
        public oldNode(string name, ref object value, oldNode parent_node = null, NodeType nodeType = NodeType.TERMINAL_NODE)
        {
            Value = value;
            NodeName = name;
            Nodes = new ObservableCollection<oldNode>();
            ParentNode = parent_node;
        }
        public void SetWorkValue(ref Work value)
        {
            Value = value;
            NodeName = value.FullName;
        }
        public void SetParentNode(oldNode node)
        {
            ParentNode = node;
        }
        /*  public Node(in string name,in  object value, Node parent_node = null, NodeType nodeType = NodeType.TERMINAL_NODE)
          {
              Value = value;
              NodeName = name;
              Nodes = new ObservableCollection<Node>();
              ParentNode = parent_node;
          }*/
        public enum NodeType
        {
            ROOT_NODE = 0,
            TERMINAL_NODE,
            COMBINE_NODE

        }

    }
}
