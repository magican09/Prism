using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldDocument : BindableBase, IbldDocument
    {
       
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }
        private string _shortName;
        public virtual string ShortName
        {
            get
            {

                return _shortName;
            }
            set { SetProperty(ref _shortName, value); }
        }
        private bldDocumentsGroup _attachedDocuments = new bldDocumentsGroup("Приложения");
        public bldDocumentsGroup AttachedDocuments
        {
            get { return _attachedDocuments; }
            set { SetProperty(ref _attachedDocuments, value); }
        }
        private int _pagesNumber;
        public int PagesNumber
        {
            get { return _pagesNumber; }
            set { SetProperty(ref _pagesNumber, value); }
        }
        private string _regId;
        public string RegId
        {
            get { return _regId; }
            set { SetProperty(ref _regId, value); }
        }
        /* private object _parent;
         public object Parent
         {
             get { return _parent; }
             set { SetProperty(ref _parent, value); }
         }
         private IbldDocumentsGroup _children;
         public IbldDocumentsGroup  Children
         {
             get { return _children; }
             set { SetProperty(ref _children, value); }
         }
         */
        public bldDocument(string name) : this()
        {
            Name = name;
        }
        public bldDocument()
        {
            AttachedDocuments.Name = "Приложения";
        }

        private Picture _imageFile;
        public Picture ImageFile
        {
            get { return _imageFile; }
            set { SetProperty(ref _imageFile, value); }
        }
    }
}
