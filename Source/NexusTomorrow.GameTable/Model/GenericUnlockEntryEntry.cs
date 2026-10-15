using NexusTomorrow.Game.Static.Entity;

namespace NexusTomorrow.GameTable.Model;

public class GenericUnlockEntryEntry
{
    public uint Id;
    public uint LocalizedTextIdDescription;
    public string SpriteIcon = string.Empty;
    public string SpritePreview = string.Empty;
    public GenericUnlockType GenericUnlockTypeEnum;
    public uint UnlockObject;
}
