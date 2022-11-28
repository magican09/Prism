﻿using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldConstruction : BindableBase, IbldConstruction, IEntityObject, ICloneable
    {
        private Guid _storedId;
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }

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
        private string _shortName;

        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set { SetProperty(ref _shortName, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }//Дата начала
        private DateTime _endTime;
        public DateTime EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }//Дата окончания
        private DateTime _netExecutionTime;
        public DateTime NetExecutionTime
        {
            get { return _netExecutionTime; }
            set { SetProperty(ref _netExecutionTime, value); }
        }//Чистое время выполнения
        private decimal _quantity;
        public decimal Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }//Количесво 
        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }//Цена за ед. 
        private bldUnitOfMeasurement _unitOfMeasurement;
        public virtual bldUnitOfMeasurement UnitOfMeasurement
        {
            get { return _unitOfMeasurement; }
            set { SetProperty(ref _unitOfMeasurement, value); }
        }//Ед. изм
        private decimal _cost;
        public decimal Cost
        {
            get { return _cost; }
            set { SetProperty(ref _cost, value); }
        }//Общая стоимость

        private decimal _laboriousness;
        public decimal Laboriousness
        {
            get { return _laboriousness; }
            set { SetProperty(ref _laboriousness, value); }
        }//Трудоемкость  чел.час/ед.изм
        private decimal _scopeOfWork;
        public decimal ScopeOfWork
        {
            get { return _scopeOfWork; }
            set { SetProperty(ref _scopeOfWork, value); }
        }
        private bldConstructionsGroup _constructions = new bldConstructionsGroup();
        public virtual bldConstructionsGroup Constructions
        {
            get { return _constructions; }
            set { SetProperty(ref _constructions, value); }
        }

        private bldWorksGroup _works = new bldWorksGroup();
        public virtual bldWorksGroup Works
        {
            get { return _works; }
            set { SetProperty(ref _works, value); }
        }

        private bldResponsibleEmployeesGroup _responsibleEmployees = new bldResponsibleEmployeesGroup();
        [NotMapped]
        public bldResponsibleEmployeesGroup? ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }

        [NavigateProperty]
        public bldObject? bldObject { get; set; }

        public bldConstruction()
        {

        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
        public void SaveAOSRsToWord(string folderPath = null)
        {
            foreach (bldWork work in Works)
            {
                work.SaveAOSRsToWord(folderPath);
            }
            foreach (bldConstruction construction in Constructions)
            {
                string construction_folder_path = System.IO.Path.Combine(folderPath, construction.ShortName); ;
                System.IO.Directory.CreateDirectory(construction_folder_path);
                construction.SaveAOSRsToWord(construction_folder_path);
            }
        }
        #region EditMethods
        public void AddWork(bldWork work)
        {
            /*   AddToCollectionCommand<bldWorksGroup, bldWork> Command =
                    new AddToCollectionCommand<bldWorksGroup, bldWork>(Works, work);*/
            AddWorkCommand Command = new AddWorkCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddResponsibleEmployee(bldResponsibleEmployee empl)
        {
            AddToCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
                 new AddToCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddConstruction(bldConstruction construction)
        {
            construction.bldObject = null; ///Спорное решение с установкой в null
            AddToCollectionCommand<bldConstructionsGroup, bldConstruction> Command =
                new AddToCollectionCommand<bldConstructionsGroup, bldConstruction>(Constructions, construction);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }

        public void RemoveWork(bldWork work)
        {
            RemoveFromCollectionCommand<bldWorksGroup, bldWork> Command =
                 new RemoveFromCollectionCommand<bldWorksGroup, bldWork>(Works, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveResponsibleEmployee(bldResponsibleEmployee empl)
        {
            RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
                 new RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveConstruction(bldConstruction constr)
        {
            RemoveFromCollectionCommand<bldConstructionsGroup, bldConstruction> Command =
                new RemoveFromCollectionCommand<bldConstructionsGroup, bldConstruction>(Constructions, constr);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }

        #endregion


    }
}
