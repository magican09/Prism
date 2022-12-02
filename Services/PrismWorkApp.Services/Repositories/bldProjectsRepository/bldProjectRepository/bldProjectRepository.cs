using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldProjectRepository : Repository<bldProject>, IbldProjectRepository

    {
        public bldProjectRepository(PlutoContext context) : base(context)
        {

        }

        public void Dispose()
        {
            this.Dispose();
        }

        public bldProject GetProjectWithObjects(Guid id)
        {
            PlutoContext.Projects.Include(pr => pr.BuildingObjects).ToList();
            return PlutoContext.Projects.FirstOrDefault();
        }
        public bldProject GetProjectWithAll()//(Guid id)
        {
            PlutoContext.Projects
                    .Include(pr => pr.BuildingObjects)
                    .ThenInclude(ob => ob.Constructions)
                    .ThenInclude(cn => cn.Works)
                    .ToList();
            /* PlutoContext.Projects
                 .Include(pr => pr.Participants)
                 .ThenInclude(pt => pt.ResponsibleEmployees)
                 .ToList();*/
            return PlutoContext.Projects.FirstOrDefault();
        }
        public List<bldProject> GetAllProjectsWhitAll()//(Guid id)
        {
            List<bldProject> all_pojects =
              PlutoContext.Projects
                  .Include(pr => pr.BuildingObjects)
                       .ThenInclude(ob => ob.Constructions)
                       .ThenInclude(cn => cn.Works)
                       .ThenInclude(wr => wr.PreviousWorks)
                       .ThenInclude(wr => wr.NextWorks)
                       .ThenInclude(wr => wr.AOSRDocuments)
                       .ThenInclude(wr => wr.AttachedDocuments)
                    .Include(pr => pr.Participants)
                        .ThenInclude(cm => cm.ConstructionCompany)
                      //  .ThenInclude(re => re.ResponsibleEmployees)
                   //     .ThenInclude(re => re.DocConfirmingTheAthority)
                .Include(pr => pr.Participants)
                        .ThenInclude(cm => cm.ConstructionCompany)
                    //    .ThenInclude(re => re.ResponsibleEmployees)
                       // .ThenInclude(re => re.Position)
                 .Include(pr => pr.Participants)
                        .ThenInclude(cm => cm.ResponsibleEmployees)
               .Include(obj => obj.BuildingObjects)
                      .ThenInclude(obj => obj.BuildingObjects)
                   .Include(pr => pr.BuildingObjects)
                      .ThenInclude(ob => ob.Constructions)
                      .ThenInclude(ob => ob.Constructions)
                  .Include(pr => pr.BuildingObjects)
                       .ThenInclude(ob => ob.Constructions)
                       .ThenInclude(cn => cn.Works)
                       .ThenInclude(wr => wr.Materials)
                       .ThenInclude(m => m.Documents)
                  .Include(pr => pr.BuildingObjects)
                       .ThenInclude(ob => ob.Constructions)
                       .ThenInclude(cn => cn.Works)
                       .ThenInclude(wr => wr.LaboratoryReports)
                    .Include(pr => pr.BuildingObjects)
                       .ThenInclude(ob => ob.Constructions)
                       .ThenInclude(cn => cn.Works)
                       .ThenInclude(wr => wr.ExecutiveSchemes)
                        .Include(pr => pr.BuildingObjects)
                       .ThenInclude(ob => ob.Constructions)
                       .ThenInclude(cn => cn.Works)
                       .ThenInclude(wr => wr.WorkArea)
                   .Include(pr => pr.BuildingObjects)/*
                         .ThenInclude(ob => ob.Constructions)
                         .ThenInclude(cn => cn.Works)
                          .ThenInclude(cn => cn.ProjectDocuments)
                      .Include(pr => pr.BuildingObjects)
                         .ThenInclude(ob => ob.Constructions)
                         .ThenInclude(cn => cn.Works)
                          .ThenInclude(cn => cn.RegulationDocuments)
                       .Include(pr => pr.BuildingObjects)
                         .ThenInclude(ob => ob.Constructions)
                         .ThenInclude(cn => cn.Works)
                         .ThenInclude(cn => cn.AOSRDocuments)
                        .Include(pr => pr.Participants)
                          .ThenInclude(cm => cm.ConstructionCompanies)
                          .ThenInclude(cm => cm.SROIssuingCompany)
                        .Include(pr => pr.Participants)
                          .ThenInclude(cm => cm.ConstructionCompanies)
                          .ThenInclude(re => re.ResponsibleEmployees)
                          .ThenInclude(re => re.Company)*/.ToList();
            return all_pojects;//out_val;
        }
        public List<bldProject> GetAll()//(Guid id)
        {
            List<bldProject> all_pojects =
                   PlutoContext.Projects
                   .Include(el => el.BuildingObjects)
                   .Include(el => el.Participants)
                   .Include(el => el.ResponsibleEmployees).ToList();

            return all_pojects;
        }
        public List<bldProject> GetProjectsAsync()//(Guid id)
        {
            List<bldProject> projects = PlutoContext.Projects.ToList();
            PlutoContext.Objects.ToList();
            PlutoContext.Constructions.ToList();
            PlutoContext.Works
               .Include(el => el.NextWorks)
               .Include(el => el.PreviousWorks).ToList();
            PlutoContext.Materials.ToList();
            PlutoContext.ProjectDocuments.ToList();
            PlutoContext.ExecutiveSchemes.ToList();
            PlutoContext.Participants.ToList();
            PlutoContext.ParticipantRoles.ToList();
            PlutoContext.ResponsibleEmployeeRoles.ToList();
            PlutoContext.ConstructionCompanies.ToList();
            PlutoContext.AOSRDocuments.ToList();
            PlutoContext.AOSRDocuments.ToList();
            PlutoContext.EmployeePositions.ToList();
            PlutoContext.LaboratoryReports.ToList();
            PlutoContext.MaterialCertificates.ToList();
            PlutoContext.RegulationtDocuments.ToList();
            PlutoContext.ResponsibleEmployees.Include(em => em.DocConfirmingTheAthority).ToList();
            PlutoContext.WorkAreas.ToList();
            PlutoContext.Projects
                .Include(p => p.Participants)
                .ThenInclude(pr => pr.ConstructionCompany)
                .ThenInclude(c => c.SROIssuingCompany).ToList();
      

            return projects;
        }

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

    }
}
