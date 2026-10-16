using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server01A3, Direction.ServerToClient)]
public sealed partial class Server01A3
{
    [UInt(3)] public byte Unknown0 { get; set; }
    [Array(CountBits = 7), UInt(16)] public List<ushort> Unknown1 { get; set; } = [];
}
