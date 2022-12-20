using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace bldCustomControlLibrary
{
    public partial class ConstructionDataGridStyle : ResourceDictionary
    {



        private void NameContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ds = this;
            ContentControl content_control = sender as ContentControl;

            TextBlock text_block = (TextBlock) LogicalTreeHelper.FindLogicalNode(sender as Visual,"nameTextBlock");
            var p1 = LogicalTreeHelper.GetParent(content_control);
            var p2 = LogicalTreeHelper.GetParent(p1);
            var p3 = LogicalTreeHelper.GetParent(p2);
            var p4 = LogicalTreeHelper.GetParent(p3);
            var p5 = LogicalTreeHelper.GetParent(p4);
          //  var p6 = LogicalTreeHelper.GetParent(p5);

            //DataGrid data_grid = (DataGrid)LogicalTreeHelper.FindLogicalNode(sender as Visual, "bldWorkDataGrid");

            text_block.Background = Brushes.Red;
            content_control.UpdateLayout();
            text_block.UpdateLayout();
       
            TextBlock text_block2 = (TextBlock)LogicalTreeHelper.FindLogicalNode(sender as Visual, "nameTextBlock");

            //   var datagrid =  FindParentByType(sender, typeof(DataGrid));


            //if (content_control.Content is Grid grid)
            //{
            //    foreach (FrameworkElement element in grid.Children)
            //    {
            //        if (element is TextBox text_box)
            //        {
            //            text_box.Background = Brushes.Red;
            //            text_box.Visibility = Visibility.Visible;
            //            text_box.UpdateLayout();
            //        }
            //        grid.UpdateLayout();
            //        //if (element is TextBlock text_block)
            //        //{
            //        //    text_block.Background = Brushes.Red;

            //        //}
            //    }
            //}
            //content_control.UpdateLayout();
            //if (datagrid is DataGrid _grid)
            //    _grid.UpdateLayout();
            //SetTextBlocksAndTextBoxs(sender);
            //(sender as ContentControl).UpdateLayout();
            //FrameworkElement parent = (FrameworkElement)VisualTreeHelper.GetParent(content_control);
            //parent.UpdateLayout();

        }
        private TextBlock textBlock;
        private void SetTextBlocksAndTextBoxs(object obj)
        {

            Visual content_contr = obj as Visual;
            int numVisulas = VisualTreeHelper.GetChildrenCount(content_contr);
            for (int ii = 0; ii < numVisulas; ii++)
            {
                Visual cont_obj = (Visual)VisualTreeHelper.GetChild(content_contr, ii);
                if (cont_obj is TextBlock text_block)
                {
                    textBlock = text_block;
                    text_block.Background = Brushes.Red;

                    text_block.Text = "sfsfsfsf";
                    //text_block.Visibility = Visibility.Hidden;
                }

                if (cont_obj is TextBox text_box)
                {

                    //  text_box.Visibility = Visibility.Visible;
                    // text_box.UpdateLayout();
                    //text_box.Focus();
                }
                if (cont_obj == null)
                    break;

                if (cont_obj != null)
                {
                    SetTextBlocksAndTextBoxs(cont_obj);
                }

            }
        }
        private object FindParentByType(object obj, Type type)
        {

            Visual v = obj as Visual;

            //    object parent = (Visual)VisualTreeHelper.GetParent(v);
            object parent = (Visual)LogicalTreeHelper.GetParent(v);
            if (parent.GetType() == type)
            {
                return parent;
            }
            else
            {
                parent = FindParentByType(parent, type);
            }
            return parent;
        }
    }
}
