
using PrismWorkApp.ProjectModel.Data.Interfaces;
using PrismWorkApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class Material: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
       
        public int Id { get; set; }
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        private string _fullname;
        public string FullName { get { return _fullname; } set { _fullname = value; OnPropertyChanged("FullName"); } }
        private string _code;
        public string Code { get { return _code; } set { _code = value; OnPropertyChanged("Code"); } }
        private double _quantity;
        public double Quantity { get { return _quantity; } set { _quantity = value; OnPropertyChanged("Quantity"); } }
        private string _measure;
        public string Measure { get { return _measure; } set { _measure = value; OnPropertyChanged("Measure"); } }
            private DateTime _date;
            public DateTime Date {
                get{
                    if (Documents.Count > 0)
                        return Documents.OrderBy(d => d.Date).FirstOrDefault().Date;
                    return _date;
                }
                set { _date = value; OnPropertyChanged("Date"); } 
            }
        private ObservableCollection<Document> _documents;
        public  ObservableCollection<Document> Documents { get { return _documents; } set { _documents = value; OnPropertyChanged("Documents"); } }
        public Material(string name ="")
        {
            Documents = new ObservableCollection<Document>();
            Name = name;
        }
    }
}
