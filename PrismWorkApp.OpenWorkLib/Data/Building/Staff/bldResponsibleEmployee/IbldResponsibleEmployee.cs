namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldResponsibleEmployee : ITemporal, IRegisterable
    {
          bldOrderDocument DocConfirmingTheAthority { get; set; }
          string NRSId { get; set; }
        // public RoleOfResponsible RoleOfResponsible { get; set; }
        //    public bldProject bldProject { get; set; }

    }
}