using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.ServerAuthAccepted, Direction.ServerToClient)]
public sealed partial class ServerAuthAccepted
{
    [UInt(32)] public uint DisconnectedForLag { get; set; }
}
