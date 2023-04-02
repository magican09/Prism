using PrismWorkApp.OpenWorkLib.Data;
using System.ComponentModel;

namespace PrismWorkApp.Modules.BuildingModule
{
    public interface IAppObjectsModel
    {
        public bldDocumentsGroup Documentation { get; set; }
        public void OnDataItemInit(DataItem dataItem, object sender, PropertyChangedEventArgs e);
    }
}