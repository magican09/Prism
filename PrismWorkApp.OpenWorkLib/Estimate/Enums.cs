namespace PrismWorkApp.OpenWorkLib.Estimate
{
    public enum ResurceType //Тип ресурса
    {
        WORKER = 0,
        EM_TIME,
        MATERIAL,
        MEC_WORKER,
        MACHINES,
        FINANCIAL,
        QUALIFICATION
    }
    public enum PositionType //Тип ресурса
    {
        TER,
        TSCC,
        COMMERCIAL
    }
    public enum BaseParamsType //Тип  параметра
    {
        PZ = 0,
        ZP,
        EM,
        ZPM,
        MR,
        VOZVR_MR,
        TANSP_MR,
        SH_MONTAJ,
        TRZ,
        TRZ_M
    }
    public enum IndexedParamsType //Тип  параметра
    {
        PZ = 0,
        ZP,
        EM,
        ZPM,
        MR,
        VOZVR_MR,
        TANSP_MR,
        SH_MONTAJ,
        TRZ,
        TRZ_M,
        DETERM,
        W_SCOPE,
        SUBORD,
        IN_ES_POSITION,
        ES_NAMB
    }
}
