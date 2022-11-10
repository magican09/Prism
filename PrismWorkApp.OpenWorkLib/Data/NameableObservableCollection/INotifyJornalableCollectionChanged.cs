using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public delegate void CollectionChangedEventHandler(object sender, CollectionChangedEventArgs e);

    public interface INotifyJornalableCollectionChanged: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyBeforeChanged;
        public event CollectionChangedEventHandler CollectionChangedBeforeRemove;
        public event CollectionChangedEventHandler CollectionChangedBeforAdd;
    }
  
    public class CollectionChangedEventArgs:EventArgs
        {
        public  IJornalable Item { get; set; }
        public CollectionChangedEventArgs(IJornalable item)
        {
            Item = item;
        }

        }

}
