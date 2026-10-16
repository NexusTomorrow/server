using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Server056D, Direction.ServerToClient)]
public sealed partial class Server056D
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [Array(CountBits = 4), UInt(32)] public List<uint> Unknown2 { get; } = [];
}
