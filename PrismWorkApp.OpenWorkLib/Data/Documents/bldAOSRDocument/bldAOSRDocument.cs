using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldAOSRDocument:bldDocument,IbldAOSRDocument,INameable, IEntityObject
    {
        private bldResponsibleEmployeesGroup _responsibleEmployees = new bldResponsibleEmployeesGroup ();
        public delegate  void  SaveToWord(bldAOSRDocument aOSRDocument, string pathToSave = "");

        public virtual bldResponsibleEmployeesGroup ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }
        private string _fullName;
        [NotJornaling]
        public override string ShortName
        { 
         get {
                this.JornalingOff();
                int wrk_name_leng = 30;
                if (bldWork?.Name.Length < wrk_name_leng) wrk_name_leng = bldWork.Name.Length;
               _fullName =  $"АОСР №{RegId} от {Date.ToString("d")} {bldWork?.Name.Substring(0, wrk_name_leng)}...";
                this.JornalingOn();
                return _fullName;
            }
            set {   }
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
        public bldAOSRDocument(string name):base(name)
        {

        }
        public bldAOSRDocument()
        {

        }
        private bldWork _work;
        [NavigateProperty]
        public virtual bldWork bldWork
        {
            get { return _work; }
            set { SetProperty(ref _work, value); }
        }
    }
}
