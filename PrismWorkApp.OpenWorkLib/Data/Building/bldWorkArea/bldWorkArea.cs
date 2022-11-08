using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldWorkArea : BindableBase, IbldWorkArea
    {
        private string _levels;
        public string Levels
        {
            get { return _levels; }
            set { SetProperty(ref _levels, value); }
        }//Отметки
        private string _axes;
        public string Axes
        {
            get { return _axes; }
            set { SetProperty(ref _axes, value); }
        } //Оси
       
        
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
      
        private string _name = "Место проведения работ";
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _shortName;
        public string ShortName
        {
            get { return _shortName; }
            set { SetProperty(ref _shortName, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }
    }
}
