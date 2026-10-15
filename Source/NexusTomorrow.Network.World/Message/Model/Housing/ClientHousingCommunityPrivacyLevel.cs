using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingCommunityPrivacyLevel, Direction.ClientToServer)]
public sealed partial class ClientHousingCommunityPrivacyLevel
{
    public Identity GuildIdentity { get; private set; } = new();
    [UInt(1)] public CommunityPrivacyLevel PrivacyLevel { get; private set; }
}
