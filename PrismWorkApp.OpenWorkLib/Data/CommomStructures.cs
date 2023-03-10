namespace PrismWorkApp.OpenWorkLib.Data
{

    public enum ParticipantRole
    {
        NONE = 0,
        DEVELOPER,
        GENERAL_CONTRACTOR,
        DISIGNER,
        BUILDER,

    }
    public enum RoleOfResponsible
    {
        CUSTOMER, //Заказчик
        GENERAL_CONTRACTOR, //Генеральный подрядчик
        GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER, //Технадзор
        AUTHOR_SUPERVISION, // Авторский надзор 
        WORK_PERFORMER,//Подрядчик
        OTHER,
        NONE
    }
    public static class bldProjectFuncions
    {
        public static bool ResponsibleRoleComparer(ParticipantRole participantRole, RoleOfResponsible roleOfResponsible)
        {
            switch (participantRole)
            {
                case ParticipantRole.DEVELOPER:
                    if (roleOfResponsible == RoleOfResponsible.CUSTOMER) return true;
                    break;
                case ParticipantRole.GENERAL_CONTRACTOR:
                    if (roleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR ||
                       roleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER) return true;
                    break;
                case ParticipantRole.DISIGNER:
                    if (roleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION) return true;

                    break;
                case ParticipantRole.BUILDER:
                    if (roleOfResponsible == RoleOfResponsible.WORK_PERFORMER) return true;
                    break;
                case ParticipantRole.NONE:
                    if (roleOfResponsible == RoleOfResponsible.OTHER || roleOfResponsible == RoleOfResponsible.NONE) return true;
                    break;
            }
            return false;
        }
    }
}
