namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ILevelable
    {
        //   public StructureLevel StructureLevel { get; set; }
          string Code { get; set; }
          void UpdateStructure();
          void ClearStructureLevel();
    }
}
