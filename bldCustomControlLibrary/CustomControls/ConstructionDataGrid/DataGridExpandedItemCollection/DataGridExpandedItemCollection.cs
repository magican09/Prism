using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace bldCustomControlLibrary
{
    public class DataGridExpandedItemCollection : ObservableCollection<DataGridExpandedItem>
    {
        public DataGridExpandedItemCollection()
        {
            CollectionChanged += OnCollectionCahged;
        }

        private void OnCollectionCahged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (DataGridExpandedItem elm in e.NewItems)
                {
                    if (elm.Object is bldConstruction construction)
                    {
                        foreach (bldWork work in construction.Works)
                        {
                            DataGridExpandedItem dataItem = new DataGridExpandedItem(work, elm.IsExpanded);
                            dataItem.Parent = elm;
                            this.Add(dataItem);
                            elm.Children.Add(dataItem);
                            dataItem.NameMagrin = new Thickness(10, 0, 0, 0);
                            dataItem.Visible = Visibility.Visible;
                        }
                    }
                }

            }
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;
            // TODO: Your logic here
        }

    }
}
