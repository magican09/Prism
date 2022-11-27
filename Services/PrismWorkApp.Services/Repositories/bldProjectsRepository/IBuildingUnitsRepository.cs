using System;

namespace PrismWorkApp.Services.Repositories
{
    public interface IBuildingUnitsRepository : IDisposable
    {
        public bldProjectRepository Projects { get; }
        public bldObjectRepository Objects { get; }
        public bldPacticipantsRepository Pacticipants { get; }
        public bldResponsibleEmployeesRepository ResponsibleEmployees { get; }
        public bldConstructionRepository Constructions { get; }
        public bldWorkRepository Works { get; }
        public int Complete();
    }
}
