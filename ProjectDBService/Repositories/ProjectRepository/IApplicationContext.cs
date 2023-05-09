using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDBService 
{
    public  interface IAppplicationContext
    {
          DbSet<bldDocument> Documents { get; set; }
          DbSet<bldAOSRDocument> AOSRDocuments { get; set; }
          DbSet<bldMaterialCertificate> MaterialCertificates { get; set; }
          DbSet<bldLaboratoryReport> LaboratoryReports { get; set; }
          DbSet<bldExecutiveScheme> ExecutiveSchemes { get; set; }
          DbSet<bldOrderDocument> OrderDocuments { get; set; }
          DbSet<bldPasportDocument> PasportDocuments { get; set; }
          DbSet<bldProjectDocument> ProjectDocuments { get; set; }
          DbSet<bldRegulationtDocument> RegulationtDocuments { get; set; }
          DbSet<Picture> Pictures { get; set; }
          DbSet<bldAggregationDocument> AggregationDocuments { get; set; }
          DbSet<TypeOfFile> TypesOfFile { get; set; }
          DbSet<FileData> FileDatas { get; set; }
    }
}
