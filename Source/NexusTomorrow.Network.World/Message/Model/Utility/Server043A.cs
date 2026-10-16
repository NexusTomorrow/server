using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Server043A, Direction.ServerToClient)]
public sealed partial class Server043A
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
