using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingCommunityRemoval, Direction.ClientToServer)]
public sealed partial class ClientHousingCommunityRemoval
{
    public Identity TargetResidence { get; private set; } = new(); // This is only ever the player's own residence
}
