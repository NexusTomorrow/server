using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.Server0076, Direction.ServerToClient)]
public sealed partial class Server0076
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Unknown0 { get; set; } = [];
}
