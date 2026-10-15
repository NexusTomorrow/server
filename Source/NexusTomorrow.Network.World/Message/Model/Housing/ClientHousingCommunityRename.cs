using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingCommunityRename)]
[WireMessage(GameMessageOpcode.ClientHousingCommunityRename, Direction.ClientToServer)]
public sealed partial class ClientHousingCommunityRename : IReadable
{
    public Identity TargetGuild { get; } = new();
    [WideString] public string Name { get; private set; }
    [UInt(1)] public bool UseServiceToken { get; private set; } // Uses ServiceToken

    public void Read(GamePacketReader reader)
    {
        TargetGuild.Read(reader);
        Name = reader.ReadWideString();
        UseServiceToken = reader.ReadBit();
    }
}
