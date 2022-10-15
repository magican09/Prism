using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldWorkUnitNormative : IbldWorkUnitNormative
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid StoredId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Code { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ShortName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FullName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public decimal Quantity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public decimal UnitPrice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bldUnitOfMeasurement UnitOfMeasurement { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public decimal Cost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public JornalRecordStatus Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid CurrentContextId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public StructureLevel StructureLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CopingEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object ParentObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ClearStructureLevel()
        {
            throw new NotImplementedException();
        }

        public void JornalingOff()
        {
            throw new NotImplementedException();
        }

        public void JornalingOn()
        {
            throw new NotImplementedException();
        }

        public void Save(object prop_id, Guid currentContextId)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(Guid currentContextId)
        {
            throw new NotImplementedException();
        }

        public void UnDo(Guid currentContextId)
        {
            throw new NotImplementedException();
        }

        public void UnDoAll(Guid currentContextId)
        {
            throw new NotImplementedException();
        }

        public void UpdateStructure()
        {
            throw new NotImplementedException();
        }
    }
}
