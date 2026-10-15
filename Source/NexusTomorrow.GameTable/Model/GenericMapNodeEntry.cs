namespace NexusTomorrow.GameTable.Model
{
    public class GenericMapNodeEntry
    {
        public uint Id;
        public uint GenericMapId;
        public uint WorldLocation2Id;
        public uint LocalizedTextIdName;
        public uint LocalizedTextIdDescription;
        public string SpritePath = string.Empty;
        public uint GenericMapNodeTypeEnum;
        public uint Flags;
    }
}
