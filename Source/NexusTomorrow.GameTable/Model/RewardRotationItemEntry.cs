namespace NexusTomorrow.GameTable.Model
{
    public class RewardRotationItemEntry
    {
        public uint Id;
        public uint RewardItemTypeEnum;
        public uint RewardItemObject;
        public uint Count;
        public string IconPath = string.Empty;
        public uint MinPlayerLevel;
        public uint WorldDifficultyFlags;
    }
}
