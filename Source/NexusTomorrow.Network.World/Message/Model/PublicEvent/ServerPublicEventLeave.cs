using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventLeave, Direction.ServerToClient)]
public sealed partial class ServerPublicEventLeave
{
    [UInt(14)] public uint PublicEventId { get; set; }
    [UInt(32)] public PublicEventRemoveReason Reason { get; set; }
}
