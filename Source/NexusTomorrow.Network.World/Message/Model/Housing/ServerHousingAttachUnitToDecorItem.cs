using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingAttachUnitToDecorItem, Direction.ServerToClient)]
public sealed partial class ServerHousingAttachUnitToDecorItem
{
    public Identity ResidenceIdentity { get; set; }
    [UInt(64)] public ulong DecorId { get; set; }
    [UInt(64)] public ulong AttachedUnitId { get; set; }
}
