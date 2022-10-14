
using PrismWorkApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class AOSRDocument : Document, INotifyPropertyChanged
    {
       /* public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
       */
        private Work _work;
        public  Work  Work { get { return _work; } set { _work = value; OnPropertyChanged("Work"); } }

        private ObservableCollection<ResponsibleEmployee> _responsibleEmployees;
        public ObservableCollection<ResponsibleEmployee> ResponsibleEmployees
        { get { return _responsibleEmployees; } set { _responsibleEmployees = value; OnPropertyChanged("ResponsibleEmployees"); } }

        public ObservableCollection<Document> _attachDocuments;
        public ObservableCollection<Document> AttachDocuments
        { get { return _attachDocuments; } set { _attachDocuments = value; OnPropertyChanged("AttachDocuments"); } }

        public AOSRDocument(string name) : base(name)
        {
            ResponsibleEmployees = new ObservableCollection<ResponsibleEmployee>();
            AttachDocuments = new ObservableCollection<Document>();
           
        }
        public AOSRDocument()
            : base()
        {
            ResponsibleEmployees = new ObservableCollection<ResponsibleEmployee>();
            AttachDocuments = new ObservableCollection<Document>();
           
        }
    }
}
