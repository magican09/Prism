using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;

namespace PrismWorkApp.Services.Repositories
{
    public class PictureRepository : Repository<Picture>, IPictureRepository
    {
        public bldProjectsPlutoContext ProjectsPlutoContext { get { return Context as bldProjectsPlutoContext; } }
        //private const string RowDataStatement = @"SELECT ImageFile.PathName() AS 'Path', GET_FILESTREAM_TRANSACTION_CONTEXT() AS 'Transaction' FROM {0} WHERE Id = @id";

        public PictureRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }

        //public List<Picture> GetAllAsync()
        //{
        //    return ProjectsPlutoContext.Pictures.ToList();
        //}

        //public void Insert(Picture entity)
        //{
        //    using (var tx = new TransactionScope())
        //    {
        //        PlutoContext.Pictures.Add(entity);
        //        PlutoContext.SaveChanges();
        //        SavePhotoData(entity);
        //        tx.Complete();
        //    }

        //}
        //public void Update(Picture entity)
        //{
        //    using (var tx = new TransactionScope())
        //    {
        //        PlutoContext.Entry(entity).State = EntityState.Modified;
        //        PlutoContext.SaveChanges();
        //        SavePhotoData(entity);
        //        tx.Complete();
        //    }

        //}
        //private void SavePhotoData(Picture entity)
        //{
        //    var table_name = PlutoContext.Model.FindEntityType(typeof(Picture)).GetTableName();

        //    var selectStatement = String.Format(RowDataStatement, table_name);

        //    var picture_id = new SqlParameter("id", entity.Id);

        //    //var rowData = 
        //    //    context.Database.SqlQuery<FileStreamRowData>(selectStatement, new SqlParameter("id", entity.Id))
        //    //        .First();

        //    //using (var destination = new SqlFileStream(rowData.Path, rowData.Transaction, FileAccess.Write))
        //    //{
        //    //    var buffer = new byte[16 * 1024];
        //    //    using (var ms = new MemoryStream(entity.ImageFile))
        //    //    {
        //    //        int bytesRead;
        //    //        while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
        //    //        {
        //    //            destination.Write(buffer, 0, bytesRead);
        //    //        }
        //    //    }
        //    //}
        //}

    }
}
