using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace bldCustomControlLibrary
{
    public partial class ConstructionDataGridStyle : ResourceDictionary
    {

        private void cell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<object> textBlocks = new ObservableCollection<object>();
            ObservableCollection<object> textBoxes = new ObservableCollection<object>();
            FindChildrenByType(sender as Visual, typeof(TextBlock), textBlocks);
            FindChildrenByType(sender as Visual, typeof(TextBox), textBoxes);
            foreach (object txt_blok in textBlocks)
                ((TextBlock)txt_blok).Visibility = Visibility.Hidden;
            foreach (object txt_box in textBoxes)
                ((TextBox)txt_box).Visibility = Visibility.Visible;



        }


        private void ContentControl_LostFocus(object sender, RoutedEventArgs e)
        {
            ObservableCollection<object> textBlocks = new ObservableCollection<object>();
            ObservableCollection<object> textBoxes = new ObservableCollection<object>();
            FindChildrenByType(sender as Visual, typeof(TextBlock), textBlocks);
            FindChildrenByType(sender as Visual, typeof(TextBox), textBoxes);
            foreach (object txt_blok in textBlocks)
                ((TextBlock)txt_blok).Visibility = Visibility.Visible;
            foreach (object txt_box in textBoxes)
                ((TextBox)txt_box).Visibility = Visibility.Hidden;
        }

        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell dg_cell = (DataGridCell)FindParentByType(sender, typeof(DataGridCell));
            // dg_cell.MouseDoubleClick += cell_MouseDoubleClick;
        }
        private void SetTextBlocksAndTextBoxs(object obj)
        {

            Visual content_contr = obj as Visual;
            int numVisulas = VisualTreeHelper.GetChildrenCount(content_contr);
            for (int ii = 0; ii < numVisulas; ii++)
            {
                Visual cont_obj = (Visual)VisualTreeHelper.GetChild(content_contr, ii);
                if (cont_obj is TextBlock text_block)
                {

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

            object parent = (Visual)VisualTreeHelper.GetParent(v);
            //    object parent = (Visual)LogicalTreeHelper.GetParent(v);
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
        private object FindChildByType(object obj, Type type)
        {
            Visual parent = obj as Visual;
            object child = null;
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                child = (Visual)VisualTreeHelper.GetChild(parent, i);

                if (child.GetType() == type)
                {
                    break;
                }
                else
                {
                    child = FindChildByType(child, type);
                }


            }

            return child;

        }
        private void FindChildrenByType(object obj, Type type, ObservableCollection<object> collection)
        {
            Visual parent = obj as Visual;
            object child = null;
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                child = (Visual)VisualTreeHelper.GetChild(parent, i);

                if (child.GetType() == type)
                {
                    collection.Add(child);
                    //  break;
                }
                else
                {
                    FindChildrenByType(child, type, collection);
                    //   collection.Add(child);
                }


            }

        }
    }
}

