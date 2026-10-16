using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server06F9, Direction.ServerToClient)]
public sealed partial class Server06F9
{
    [UInt(15)] public uint Unknown0 { get; set; }
}
