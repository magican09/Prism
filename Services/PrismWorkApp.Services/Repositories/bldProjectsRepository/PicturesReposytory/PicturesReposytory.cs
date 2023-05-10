using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Services.Repositories
{
    public class PicturesReposytory : Repository<Picture>, IPicturesReposytory
    {
       // public bldProjectsPlutoContext ProjectsPlutoContext { get { return Context as bldProjectsPlutoContext; } }
        public PicturesReposytory(bldProjectsPlutoContext context) : base(context)
        {
        }
        public void Dispose()
        {
            this.Dispose();
        }
    }
}
