using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public abstract class bldDocument : BindableBase, IbldDocument 
    {
        //private  Type _type;
        //public Type Type
        //{
        //    get { return _type; }
        //    set { SetProperty(ref _type, value); }
        //}
        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
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
            get { return _shortName; }
            set { SetProperty(ref _shortName, value); }
        }
        private bldDocumentsGroup _attachedDocuments ;
        [CreateNewWhenCopy]
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
        public bldDocument(string name) : this()
        {
            Name = name;
        }
        public bldDocument()
        {
           AttachedDocuments = new bldDocumentsGroup("Приложения");
          ParentDocuments = new ObservableCollection<bldDocument>();
        }
        private ObservableCollection<bldDocument> _parentDocuments ;
        public ObservableCollection<bldDocument> ParentDocuments
        {
            get { return _parentDocuments; }
            set { SetProperty(ref _parentDocuments, value); }
        }
        private Picture  _imageFile;
        public Picture  ImageFile
        {
            get { return _imageFile; }
            set { SetProperty(ref _imageFile, value); }
        }
        private bool _IsHaveImageFile = false;
        public bool IsHaveImageFile
        {
            get { return _IsHaveImageFile; }
            set { SetProperty(ref _IsHaveImageFile, value); }
        }
        #region Methods 
        
        #endregion

        #region Commands 
        public TEntity AddNewDocument<TEntity>(TEntity new_doc) where TEntity : IbldDocument, new()
        {
            AddDocumentCommand<TEntity> Command = new AddDocumentCommand<TEntity>(this, new_doc);
            InvokeUnDoReDoCommandCreatedEvent(Command);
            return new_doc;
        }
        public bldDocument AddCreatedBasedOnNewDocument(IbldDocument sample_doc) 
        {
            bldDocument new_doc = (bldDocument)sample_doc.Clone();
          //  new_doc.Id = Guid.NewGuid();
            new_doc.IsHaveImageFile = false;
            AddDocumentCommand<bldDocument> Command = new AddDocumentCommand<bldDocument>(this, new_doc);
            InvokeUnDoReDoCommandCreatedEvent(Command);
            return new_doc;
        }
        public void RemoveDocument(IbldDocument removed_doc)
        {
            RemoveDocumentCommand Command = new RemoveDocumentCommand(this, removed_doc);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddDocument(IbldDocument added_doc)
        {
            AddDocumentCommand<IbldDocument> Command = new AddDocumentCommand<IbldDocument>(this, added_doc);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        #endregion
    }
}
