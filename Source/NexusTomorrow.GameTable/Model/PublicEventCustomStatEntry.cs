using NexusTomorrow.Game.Static.PublicEvent;

namespace NexusTomorrow.GameTable.Model
{
    public class PublicEventCustomStatEntry
    {
        public uint Id;
        public PublicEventType PublicEventTypeEnum;
        public uint PublicEventId;
        public uint StatIndex;
        public uint LocalizedTextIdStatName;
        public string IconPath = string.Empty;
    }
}
