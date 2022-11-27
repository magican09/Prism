
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class Document : oldNode, INotifyPropertyChanged
    {
        /* public event PropertyChangedEventHandler PropertyChanged;
          public void OnPropertyChanged([CallerMemberName] string prop = "")
          {
              if (PropertyChanged != null)
                  PropertyChanged(this, new PropertyChangedEventArgs(prop));
          }*/
        public Guid Id { get; set; }
        private string _name;
        public string Name { get { return _name; } set { _name = value; NodeName = $"{RegId}: {_name}"; OnPropertyChanged("Name"); } }
        private string _fullname;
        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; OnPropertyChanged("FullName"); }
        }
        private DateTime _date;
        public DateTime Date { get { return _date; } set { _date = value; OnPropertyChanged("Date"); } }
        private string _code;
        public string Code { get { return _code; } set { _code = value; OnPropertyChanged("Code"); } }
        public Document(string name)
        {
            Name = name;

        }
        public Document()
        {

        }

        //  public ObservableCollection<IRequisite> Requisites { get; set; }
        private string _printingName;
        public string PrintingName { get { return _printingName; } set { _printingName = value; OnPropertyChanged("PrintingName"); } }
        private ObservableCollection<Document> _attachDocuments;
        public ObservableCollection<Document> AttachDocuments
        { get { return _attachDocuments; } set { _attachDocuments = value; OnPropertyChanged("AttachDocuments"); } }
        private int _pagesNumber;
        public int PagesNumber { get { return _pagesNumber; } set { _pagesNumber = value; OnPropertyChanged("PagesNumber"); } }
        private string _regId;
        public string RegId { get { return _regId; } set { _regId = value; NodeName = $"{RegId}: {_name}"; OnPropertyChanged("RegId"); } }

    }


}

