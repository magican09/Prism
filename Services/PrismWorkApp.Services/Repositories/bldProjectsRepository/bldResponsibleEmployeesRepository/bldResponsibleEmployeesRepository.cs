﻿using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldResponsibleEmployeesRepository : Repository<bldResponsibleEmployee>

    {
        public bldResponsibleEmployeesRepository(PlutoContext context) : base(context)
        {

        }

        public void Dispose()
        {
            this.Dispose();
        }

        public List<bldResponsibleEmployee> GetAllResponsibleEmployees()//(Guid id)
        {
            PlutoContext.ResponsibleEmployees
                    //  .Include(re => re.bldParticipant);
                    .Include(re => re.Employee)
                    .ThenInclude(re => re.Company);

            return PlutoContext.ResponsibleEmployees.ToList();//out_val;
        }
        public List<bldResponsibleEmployee> GetAllAsync()
        {
            return PlutoContext.ResponsibleEmployees.ToList();
        }

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

    }
}
