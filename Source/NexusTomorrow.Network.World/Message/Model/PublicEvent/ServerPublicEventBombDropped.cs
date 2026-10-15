using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventBombDropped, Direction.ServerToClient)]
public sealed partial class ServerPublicEventBombDropped
{
    [UInt(32)] public uint CarrierUnitId { get; set; }
    [UInt(1)] public bool Unknown { get; set; }
}
