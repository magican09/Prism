using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldParticipant : BindableBase, IbldParticipant, IEntityObject
    {
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
        private bldConstructionCompanyGroup _constructionCompanies = new bldConstructionCompanyGroup();
        public bldConstructionCompanyGroup ConstructionCompanies
        {
            get { return _constructionCompanies; }
            set { SetProperty(ref _constructionCompanies, value); }
        }
        private string _roleName;
        [NotMapped]
        public string RoleName
        {
            get
            {
                switch (Role)
                {
                    case ParticipantRole.DEVELOPER:
                        _roleName = "Застройщик";

                        break;
                    case ParticipantRole.GENERAL_CONTRACTOR:
                        _roleName = "Генподрядчик";

                        break;
                    case ParticipantRole.DISIGNER:
                        _roleName = "Проектировщик";
                        break;
                    case ParticipantRole.BUILDER:
                        _roleName = "Подрядчик";
                        break;
                    default:
                        _roleName = "Не определено";
                        break;
                }
                return _roleName;
            }
            set { SetProperty(ref _roleName, value); }
        }
        private string _roleFullName;
        [NotMapped]
        public string RoleFullName
        {
            get
            {
                switch (Role)
                {
                    case ParticipantRole.DEVELOPER:
                        _roleFullName = "Застройщик(технический заказчик, эксплуатирующая организация или региональный оператор)";
                        break;
                    case ParticipantRole.GENERAL_CONTRACTOR:
                        _roleFullName = "Генеральный подрядчик(лицо, осуществляющее строительство)";
                        break;
                    case ParticipantRole.DISIGNER:
                        _roleFullName = "Проектировщик (Лицо, осуществляющее подготовку проектной документации";
                        break;
                    case ParticipantRole.BUILDER:
                        _roleFullName = "Подрядчик(лицо, выполнившеее работы)";
                        break;
                    default:
                        _roleFullName = "Не определено";
                        break;
                }
                return _roleFullName;
            }
            set { SetProperty(ref _roleFullName, value); }
        }
        private ParticipantRole _role;
        public ParticipantRole Role
        {
            get { return _role; }
            set
            {
                SetProperty(ref _role, value);
            }
        }
        private bldResponsibleEmployeesGroup _responsibleEmployees = new bldResponsibleEmployeesGroup();
        public bldResponsibleEmployeesGroup ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }



        public bldObjectsGroup BuildingObjects { get; set; }
        public bldConstructionsGroup Constructions { get; set; }
        public bldWorksGroup Works { get; set; }
        [NavigateProperty]
        public bldProject bldProject { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
