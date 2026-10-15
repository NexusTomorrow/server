namespace NexusTomorrow.GameTable.Model
{
    public class LevelUpUnlockEntry
    {
        public uint Id;
        public uint LevelUpUnlockSystemEnum;
        public uint Level;
        public uint LevelUpUnlockTypeId;
        public uint LocalizedTextIdDescription;
        public string DisplayIcon = string.Empty;
        public uint PrerequisiteId;
        public uint LevelUpUnlockValue;
    }
}
