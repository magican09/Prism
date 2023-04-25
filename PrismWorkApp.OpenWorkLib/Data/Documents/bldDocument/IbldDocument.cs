using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldDocument : IBindableBase
    {
        bldDocumentsGroup AttachedDocuments { get; set; }
        DateTime Date { get; set; }
        string FullName { get; set; }
        Picture ImageFile { get; set; }
        int PagesNumber { get; set; }
        string RegId { get; set; }
        string ShortName { get; set; }
        public bool IsHaveImageFile { get; set; }
    }
}