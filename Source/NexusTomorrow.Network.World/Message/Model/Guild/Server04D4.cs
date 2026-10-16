using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04D4, Direction.ServerToClient)]
public sealed partial class Server04D4
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 8), UInt(64)] public List<ulong> Unknown1 { get; set; } = [];
}
