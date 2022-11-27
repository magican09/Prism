namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldConstructionCompany : bldCompany, IbldConstructionCompany, IEntityObject
    {
        private bldCompany _sROIssuingCompany;
        public bldCompany SROIssuingCompany
        {
            get { return _sROIssuingCompany; }
            set { SetProperty(ref _sROIssuingCompany, value); }
        }
        private bldResponsibleEmployeesGroup _responsibleEmployees = new bldResponsibleEmployeesGroup();
        public bldResponsibleEmployeesGroup ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }

        //  public Guid bldParticipantId { get; set; }
        [NavigateProperty]
        public bldParticipantsGroup bldParticipants { get; set; }
    }
}
