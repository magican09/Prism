namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldParticipantRole : BindableBase, IKeyable, INameable
    {

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
        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set { SetProperty(ref _shortName, value); }
        }
        //private string _fullName;
        //public string FullName
        //{
        //    get
        //    {
        //        switch (Role)
        //        {
        //            case ParticipantRole.DEVELOPER:
        //                _fullName = "Застройщик(технический заказчик, эксплуатирующая организация или региональный оператор)";
        //                break;
        //            case ParticipantRole.GENERAL_CONTRACTOR:
        //                _fullName = "Генеральный подрядчик(лицо, осуществляющее строительство)";
        //                break;
        //            case ParticipantRole.DISIGNER:
        //                _fullName = "Проектировщик (Лицо, осуществляющее подготовку проектной документации";
        //                break;
        //            case ParticipantRole.BUILDER:
        //                _fullName = "Подрядчик(лицо, выполнившеее работы)";
        //                break;
        //            default:
        //                _fullName = "Не определено";
        //                break;
        //        }
        //        return _fullName;
        //    }
        //    set { SetProperty(ref _fullName, value); }
        //}
        //private string _name;
        //public string Name
        //{
        //    get
        //    {
        //        switch (Role)
        //        {
        //            case ParticipantRole.DEVELOPER:
        //                _name = "Застройщик";

        //                break;
        //            case ParticipantRole.GENERAL_CONTRACTOR:
        //                _name = "Генподрядчик";

        //                break;
        //            case ParticipantRole.DISIGNER:
        //                _name = "Проектировщик";
        //                break;
        //            case ParticipantRole.BUILDER:
        //                _name = "Подрядчик";
        //                break;
        //            default:
        //                _name = "Не определено";
        //                break;
        //        }
        //        return _name;
        //    }
        //    set { SetProperty(ref _name, value); }
        //}
        private ParticipantRole _roleCode;
        public ParticipantRole RoleCode
        {
            get { return _roleCode; }
            set
            {
                SetProperty(ref _roleCode, value);
                OnPropertyChanged("Name");
                OnPropertyChanged("FullName");

            }
        }
        public bldParticipantRole()
        {

        }
        public bldParticipantRole(ParticipantRole role)
        {
            RoleCode = role;
        }

    }
}
