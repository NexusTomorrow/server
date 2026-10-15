using NexusTomorrow.GameTable.Static;

namespace NexusTomorrow.GameTable.Model
{
    public class UnitProperty2Entry
    {
        public uint Id;
        public string Description = string.Empty;
        public string EnumName = string.Empty;
        public float DefaultValue;
        public uint LocalizedTextId;
        public float ValuePerPoint;
        public UnitPropertyFlags Flags;
        public uint TooltipDisplayOrder;
        public uint ProfiencyFlagEnum;
        public uint ItemCraftingGroupFlagBitMask;
        public uint EquippedSlotFlags;
    }
}
