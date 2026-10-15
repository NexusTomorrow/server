using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

// Sent either the player's identity or the player's name, never both
[WireMessage(GameMessageOpcode.ClientHousingNeighbourSetPermission, Direction.ClientToServer)]
public sealed partial class ClientHousingNeighbourSetPermission
{
    public Identity Identity { get; private set; } = new();
    [WideString] public string Name { get; private set; }
    [UInt(32)] public NeighborPermissionLevel Permissions { get; private set; }
}
