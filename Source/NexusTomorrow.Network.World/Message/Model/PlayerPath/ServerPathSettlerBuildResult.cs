using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server066C, Direction.ServerToClient)]
public sealed partial class Server066C
{
    [UInt(32)] public uint Unknown0 { get; set; }

    [UInt(15)] public uint Unknown1 { get; set; }
    [UInt(14)] public uint Unknown2 { get; set; }
}
