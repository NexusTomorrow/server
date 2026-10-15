using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingCommunityPrivacyLevel)]
[WireMessage(GameMessageOpcode.ClientHousingCommunityPrivacyLevel, Direction.ClientToServer)]
public sealed partial class ClientHousingCommunityPrivacyLevel : IReadable
{
    public Identity GuildIdentity { get; private set; } = new();
    [UInt(1)] public CommunityPrivacyLevel PrivacyLevel { get; private set; }

    public void Read(GamePacketReader reader)
    {
        GuildIdentity.Read(reader);
        PrivacyLevel = reader.ReadEnum<CommunityPrivacyLevel>(1);
    }
}
