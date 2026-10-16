using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03C0, Direction.ServerToClient)]
public sealed partial class Server03C0
{
    [UInt(8), Count] public byte Unknown0 { get; set; }
    [Array(CountFrom = nameof(Unknown0)), UInt(64)] public List<ulong> Unknown1 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown2 { get; set; } = [];
}
