namespace NexusTomorrow.GameTable.Model
{
    public class Spell4ThresholdsEntry
    {
        public uint Id;
        public uint Spell4IdParent;
        public uint Spell4IdToCast;
        public uint OrderIndex;
        public uint ThresholdDuration;
        public uint VitalEnumCostType00;
        public uint VitalEnumCostType01;
        public uint VitalCostValue00;
        public uint VitalCostValue01;
        public uint LocalizedTextIdTooltip;
        public string IconReplacement = string.Empty;
        public uint VisualEffectId;
    }
}
