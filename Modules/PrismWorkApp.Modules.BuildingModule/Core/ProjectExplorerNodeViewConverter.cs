//using PrismWorkApp.ProjectModel.Data.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public class ProjectExplorerNodeViewConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //oldNode node = (oldNode)values[0];
            string out_str = "!!!!!!!!";
            //var node_value_type = node.Value?.GetType();
            ///*  if (node.NodeName!=""  && node.Value != null && !node_value_type.Name.Contains("ObservableCollection"))
            //      out_str = $"{node.NodeName}: {((IItem)node.Value)?.Name}.";
            //  else if (node.Value != null && !node_value_type.Name.Contains("ObservableCollection"))
            //      out_str = $"{((IItem)node.Value)?.Name}.";
            //  else
            //      out_str = $"{node.NodeName}";
            //  */
            //switch (node.Type)
            //{
            //    case oldNode.NodeType.ROOT_NODE:
            //        out_str = $"{node.NodeName}";
            //        break;
            //    case oldNode.NodeType.TERMINAL_NODE:
            //        out_str = $"{node.NodeName}";
            //        break;
            //    case oldNode.NodeType.COMBINE_NODE:
            //        //  out_str = $"{node.NodeName}: {(node.Value)?.Name}.";
            //        out_str = "Заглушка!!";
            //        break;
            //    default:
            //        out_str = "unDefined!!!!!";
            //        break;
            //}


            return out_str;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
