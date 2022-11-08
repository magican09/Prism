using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldParticipant : BindableBase, IbldParticipant, IEntityObject
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
            get { return _shortName; }
            set { SetProperty(ref _shortName, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }
       
        private bldConstructionCompanyGroup _constructionCompanies = new bldConstructionCompanyGroup();
        public  bldConstructionCompanyGroup ConstructionCompanies
        {
            get { return _constructionCompanies; }
            set { SetProperty(ref _constructionCompanies, value); }
        }
       /* private bldConstructionCompany _company;
        public virtual  bldConstructionCompany Company 
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }   
        */
        private ParticipantRole _role;
        public ParticipantRole Role
        {
            get { return _role; }
            set 
            {
               
                    switch (value)
                    {
                        case ParticipantRole.DEVELOPER:
                        FullName  = "Застройщик(технический заказчик, эксплуатирующая организация или региональный оператор)";
                        Name = "Застройщик";

                        break;
                        case ParticipantRole.GENERAL_CONTRACTOR:
                        FullName = "Генеральный подрядчик(лицо, осуществляющее строительство)";
                        Name = "Генподрядчик";

                        break;
                        case ParticipantRole.DISIGNER:
                        FullName = "Проектировщик (Лицо, осуществляющее подготовку проектной документации";
                        Name = "Проектировщик";
                        break;
                        case ParticipantRole.BUILDER:
                        FullName = "Подрядчик(лицо, выполнившеее работы)";
                       Name = "Подрядчик";
                        break;
                        default:
                        Name = "Не определено";
                            break;
                    }
                    SetProperty(ref _role, value);
            }
        }
        private bldResponsibleEmployeesGroup _responsibleEmployees = new bldResponsibleEmployeesGroup();
        public  bldResponsibleEmployeesGroup ResponsibleEmployees
         {
             get { return _responsibleEmployees; }
             set { SetProperty(ref _responsibleEmployees, value); }
         }
         
        [NavigateProperty]
        public bldProject bldProject { get; set; }
       

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
