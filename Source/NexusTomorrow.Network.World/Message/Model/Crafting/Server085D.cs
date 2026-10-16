using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Server085D, Direction.ServerToClient)]
public sealed partial class Server085D
{
    [UInt(4)] public byte Unknown0 { get; set; }
}
