using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

[WireMessage(GameMessageOpcode.Server012C, Direction.ServerToClient)]
public sealed partial class Server012C
{
    [UInt(15)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(17)] public uint Unknown2 { get; set; }
}
