namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldAggregationDocument : bldDocument, IbldCertificateDocument, IEntityObject
    {
        public bldAggregationDocument(string name) : base(name)
        {
            this.Code = "вед";

        }
        public bldAggregationDocument() : base()
        {
            this.Code = "вед";
        }
    }

}