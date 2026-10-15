using NexusTomorrow.Game.Static.Hazard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.ServerHazardAction, Direction.ServerToClient)]
public sealed partial class ServerHazardAction
{
    [UInt(14)] public uint HazardId { get; set; }
    [UInt(3)] public HazardAction Action { get; set; }
}
