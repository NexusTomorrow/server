using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing
{
    [Message(GameMessageOpcode.ClientHousingCommunityRename)]
    public class ClientHousingCommunityRename : IReadable
    {
        public Identity TargetGuild { get; } = new();
        public string Name { get; private set; }
        public bool UseServiceToken { get; private set; } // Uses ServiceToken

        public void Read(GamePacketReader reader)
        {
            TargetGuild.Read(reader);
            Name = reader.ReadWideString();
            UseServiceToken = reader.ReadBit();
        }
    }
}
