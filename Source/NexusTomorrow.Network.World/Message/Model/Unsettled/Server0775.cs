using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0775, Direction.ServerToClient)]
public sealed partial class Server0775
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Unknown0 { get; set; } = [];
}
