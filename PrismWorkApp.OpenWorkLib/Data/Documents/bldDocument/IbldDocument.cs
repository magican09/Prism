using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldDocument : IRegisterable, IEntityObject, INameable, IBindableBase, ICloneable
    {
        bldDocumentsGroup AttachedDocuments { get; set; }
        DateTime Date { get; set; }
        string FullName { get; set; }
        Picture ImageFile { get; set; }
        int PagesNumber { get; set; }
        string RegId { get; set; }
        string ShortName { get; set; }
    }
}