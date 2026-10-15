namespace NexusTomorrow.GameTable.Model
{
    public class RealmDataCenterEntry
    {
        public uint Id;
        public uint Flags;
        public uint DeploymentRegionEnum;
        public uint DeploymentTypeEnum;
        public uint LocalizedTextId;
        public string AuthServer = string.Empty;
        public string NcClientAuthServer = string.Empty;
        public string NcRedirectUrlTemplate = string.Empty;
        public string NcRedirectUrlTemplateSignature = string.Empty;
        public string NcAppGroupCode = string.Empty;
        public uint NcProgramAuth;
        public string SteamSignatureUrlTemplate = string.Empty;
        public string SteamNCoinUrlTemplate = string.Empty;
        public string StoreBannerDataUrlTemplate = string.Empty;
    }
}
