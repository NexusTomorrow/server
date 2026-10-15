namespace NexusTomorrow.GameTable.Model
{
    public class Creature2ArcheTypeEntry
    {
        public uint Id;
        public string Icon = string.Empty;
        [GameTableFieldArray(200)]
        public float[] UnitPropertyMultiplier = [];
    }
}
