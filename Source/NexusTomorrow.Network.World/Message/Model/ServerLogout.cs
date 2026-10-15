using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerLogout, Direction.ServerToClient)]
public sealed partial class ServerLogout
{
    [UInt(1)] public bool Requested { get; set; }
    [UInt(5)] public LogoutReason Reason { get; set; }
}
