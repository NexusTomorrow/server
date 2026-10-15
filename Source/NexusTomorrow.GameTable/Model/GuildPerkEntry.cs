namespace NexusTomorrow.GameTable.Model
{
    public class GuildPerkEntry
    {
        public uint Id;
        public uint LocalizedTextIdTitle;
        public uint LocalizedTextIdDescription;
        public string LuaSprite = string.Empty;
        public string LuaName = string.Empty;
        public uint PurchaseInfluenceCost;
        public uint ActivateInfluenceCost;
        public uint Spell4IdActivate;
        public uint GuildPerkIdRequired00;
        public uint GuildPerkIdRequired01;
        public uint GuildPerkIdRequired02;
        public uint AchievementIdRequired;
    }
}
