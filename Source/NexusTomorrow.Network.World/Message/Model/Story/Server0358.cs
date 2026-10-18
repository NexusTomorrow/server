using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Server0358, Direction.ServerToClient)]
public sealed partial class Server0358
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
