﻿using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public abstract class bldDocument : BindableBase, IbldDocument,IEntityObject
    {

        private DateTime _date;
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
        private bldDocumentsGroup  _attachedDocuments = new bldDocumentsGroup ("Приложения");
        public bldDocumentsGroup  AttachedDocuments
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
            AttachedDocuments.Parents.Add(this);
        }
        public bldDocument()
        {
            AttachedDocuments.Owner = this;
        }
        private  ObservableCollection<bldDocument> _parentDocuments = new ObservableCollection<bldDocument>();
        public ObservableCollection<bldDocument> ParentDocuments
        {
            get { return _parentDocuments; }
            set { SetProperty(ref _parentDocuments, value); }
        }
        private Picture _imageFile;
        public Picture ImageFile
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

        #region Commands 
        public TEntity AddNewDocument<TEntity>() where TEntity:IbldDocument,new()
        {
            TEntity new_doc = new TEntity();
            AddDocumentCommand<TEntity> Command = new AddDocumentCommand<TEntity>(this, new_doc);
            InvokeUnDoReDoCommandCreatedEvent(Command);
            return new_doc;
        }
        public TEntity AddCreatedBasedOnNewDocument<TEntity>(TEntity sample_doc) where TEntity : IbldDocument, new()
        {
            TEntity new_doc =(TEntity)sample_doc.Clone();
            new_doc.Id = Guid.NewGuid();
            AddDocumentCommand<TEntity> Command = new AddDocumentCommand<TEntity>(this, new_doc);
            InvokeUnDoReDoCommandCreatedEvent(Command);
            return new_doc;
        }
        public void  RemoveDocument(IbldDocument removed_doc) 
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
