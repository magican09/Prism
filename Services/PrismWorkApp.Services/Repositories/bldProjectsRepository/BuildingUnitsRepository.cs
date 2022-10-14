using PrismWorkApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
    public class BuildingUnitsRepository:IBuildingUnitsRepository
    {
        
        public bldProjectRepository Projects { get; }
        public bldObjectRepository Objects { get; }
        public bldPacticipantsRepository Pacticipants { get; }
        public bldResponsibleEmployeesRepository  ResponsibleEmployees { get; }
        public bldConstructionRepository Constructions { get; }
        public bldWorkRepository Works { get; }

        private readonly PlutoContext _context;

        public BuildingUnitsRepository(PlutoContext context)
        {
            _context = context;
            Projects = new bldProjectRepository(_context);
            Objects = new bldObjectRepository(_context);
            Pacticipants = new bldPacticipantsRepository(_context);
            ResponsibleEmployees = new bldResponsibleEmployeesRepository(_context);
            Constructions = new bldConstructionRepository(_context);
            Works = new bldWorkRepository(_context);
        }
        public int Complete()
        {
        //    _context.Attach(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
