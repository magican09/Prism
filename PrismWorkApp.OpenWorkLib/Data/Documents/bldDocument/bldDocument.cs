﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldDocument :  BindableBase, IbldDocument,IEntityObject
    {
        private Guid _id =  Guid.NewGuid();
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value);  }
        }
        private Guid _storedId;
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }
        private string _code;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }//Код
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
            get {
                
                return _shortName;
            }
            set { SetProperty(ref _shortName, value); }
        }
        private bldDocumentsGroup _attachedDocuments = new bldDocumentsGroup("Приложения");
        public   bldDocumentsGroup AttachedDocuments
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
        public bldDocument(string name): this()
        {
            Name = name;
        }
        public bldDocument()
        {
            AttachedDocuments.Name = "Приложения";
        }
     }
}