﻿using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldObject: BindableBase ,IbldObject, IEntityObject,IJornalable,ILevelable
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
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
         /*       int short_name_leng = 40;
                string short_name = "";
                if (Name?.Length < short_name_leng) 
                    short_name = $"{Name}";
                else 
                    short_name = $"{Name?.Substring(0, short_name_leng)}";
                SetProperty(ref _shortName, short_name);*/
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
      
        public virtual bldConstructionsGroup? Constructions 
        {
            get { return _constructions; }
            set { SetProperty(ref _constructions, value); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }
        private bldObjectsGroup _buildingObjects  =  new bldObjectsGroup();
        public virtual bldObjectsGroup BuildingObjects
        {
            get { return _buildingObjects; }
            set { SetProperty(ref _buildingObjects, value); }
        }
        //   [NotMapped]
        //  public  List<string> List_prop { get; set; } = new List<string>();
        
        private bldProject _bldProject ;
        [NavigateProperty]
        public virtual bldProject? bldProject
        {
            get { return _bldProject; }
            set { SetProperty(ref _bldProject, value); }
        }

   
        public override bool Equals(object? obj)
        {
            if(obj is IEntityObject)
            {
                if (((IEntityObject)obj).Id == this.Id && this.Id!=Guid.Empty) return true;
                if (((IEntityObject)obj).StoredId == this.StoredId && this.StoredId != Guid.Empty) return true;
             }
            return false;
        }
        public override int GetHashCode()
        {
            if (this.Id != Guid.Empty) return this.Id.GetHashCode();
            if (this.StoredId != Guid.Empty) return this.Id.GetHashCode();
            return this.Id.GetHashCode();
        }
        public object Clone()
        {
            bldObject val = new bldObject();
            val = (bldObject)MemberwiseClone();
            return val;
        }
        

       
    }
}