using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server03DC, Direction.ServerToClient)]
public sealed partial class Server03DC
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Unknown0 { get; set; } = [];
}
