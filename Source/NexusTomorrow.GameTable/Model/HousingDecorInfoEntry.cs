namespace NexusTomorrow.GameTable.Model
{
    public class HousingDecorInfoEntry
    {
        public uint Id;
        public uint HousingDecorTypeId;
        public uint HookTypeId;
        public uint LocalizedTextIdName;
        public uint Flags;
        public uint HookAssetId;
        public uint Cost;
        public uint CostCurrencyTypeId;
        public uint Creature2IdActiveProp;
        public uint PrerequisiteIdUnlock;
        public uint Spell4IdInteriorBuff;
        public uint HousingDecorLimitCategoryId;
        public string AltPreviewAsset = string.Empty;
        public string AltEditAsset = string.Empty;
        public float MinScale;
        public float MaxScale;
    }
}
