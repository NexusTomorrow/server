using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.Server0591, Direction.ServerToClient)]
public sealed partial class Server0591
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
