using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ServerForceKick, Direction.ServerToClient)]
public sealed partial class ServerForceKick
{
    [UInt(5)] public ForceKickReason Reason { get; set; } = ForceKickReason.WorldDisconnect;
}
