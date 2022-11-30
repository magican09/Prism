using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldMaterial : bldResource, IbldMaterial, IEntityObject
    {
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private bldDocumentsGroup _documents = new bldDocumentsGroup("Документация");
        public bldDocumentsGroup Documents
        {
            get { return _documents; }
            set { SetProperty(ref _documents, value); }
        }
        public bldMaterial()
        {
            Documents.Name = "Документация";
        }

    }
}
