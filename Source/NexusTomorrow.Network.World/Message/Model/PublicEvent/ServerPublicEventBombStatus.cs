using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventBombStatus, Direction.ServerToClient)]
public sealed partial class ServerPublicEventBombStatus
{
    [NotOnWire] public PublicEventTeam Team { get; set; }
    [UInt(32)] public uint CarrierUnitId { get; set; }
    [UInt(32)] public uint TimeInMs { get; set; }
}
