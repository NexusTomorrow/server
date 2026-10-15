using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingOwnership, Direction.ServerToClient)]
public sealed partial class ServerHousingOwnership
{
    public Identity ResidenceIdentity { get; set; }
    [UInt(32)] public OwnershipType Ownership { get; set; }
}
