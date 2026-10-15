using NexusTomorrow.GameTable.Static;

namespace NexusTomorrow.GameTable.Model
{
    public class ChatChannelEntry
    {
        public uint Id;
        public string EnumName = string.Empty;
        public string UniversalCommand00 = string.Empty;
        public string UniversalCommand01 = string.Empty;
        public uint LocalizedTextIdName;
        public uint LocalizedTextIdCommand;
        public uint LocalizedTextIdAbbreviation;
        public uint LocalizedTextIdAlternate00;
        public uint LocalizedTextIdAlternate01;
        public ChatChannelEntryFlags Flags;
    }
}
