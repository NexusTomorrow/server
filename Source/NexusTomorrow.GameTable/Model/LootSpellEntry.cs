namespace NexusTomorrow.GameTable.Model
{
    public class LootSpellEntry
    {
        public uint Id;
        public uint LootSpellTypeEnum;
        public uint LootSpellPickupEnumFlags;
        public uint Creature2Id;
        public string ButtonIcon = string.Empty;
        public uint LocalizedTextIdName;
        public uint LocalizedTextIdDescription;
        public uint VisualEffectId;
        public uint Data;
        public uint DataValue;
    }
}
