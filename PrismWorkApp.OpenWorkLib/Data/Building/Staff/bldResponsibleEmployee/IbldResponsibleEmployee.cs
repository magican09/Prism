namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldResponsibleEmployee : ITemporal, IRegisterable
    {
        public bldOrderDocument DocConfirmingTheAthority { get; set; }
        public string NRSId { get; set; }
        // public RoleOfResponsible RoleOfResponsible { get; set; }
        //    public bldProject bldProject { get; set; }

    }
}