using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingVisit, Direction.ClientToServer)]
public sealed partial class ClientHousingVisit
{
    [UInt(64)] public ulong Unused { get; private set; } // always 0
    public Identity IdentityToVisit { get; } = new(); // can be a player Identity or a residence Identity
    [WideString] public string PlayerToVisitName { get; private set; }
    public Identity CommunityToVisitIdentity { get; } = new();
    [WideString] public string CommunityToVisitName { get; private set; }
}
