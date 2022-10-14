namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldConstructionCompany
    {
        public bldCompany SROIssuingCompany { get; set; }
        public  bldResponsibleEmployeesGroup ResponsibleEmployees { get; set; }
    }
}