using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Server04FE, Direction.ServerToClient)]
public sealed partial class Server04FE
{
    [UInt(32), Count] public uint Unknown0 { get; set; }
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown1 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown2 { get; set; } = [];
}
