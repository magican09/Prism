using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule
{
    public interface IAppObjectsModel
    {
        public NameableObservableCollection<IEntityObject> AllModels { get;  }
        public bldDocumentsGroup Documentation { get; set; }
    }
}