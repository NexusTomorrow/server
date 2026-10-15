using NexusTomorrow.Game.Static.Entity;

namespace NexusTomorrow.GameTable.Model
{
    public class GenericUnlockEntryEntry
    {
        public uint Id;
        public uint LocalizedTextIdDescription;
        public string SpriteIcon;
        public string SpritePreview;
        public GenericUnlockType GenericUnlockTypeEnum;
        public uint UnlockObject;
    }
}
