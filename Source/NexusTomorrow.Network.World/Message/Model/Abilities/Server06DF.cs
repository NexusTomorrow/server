using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server06DF, Direction.ServerToClient)]
public sealed partial class Server06DF
{
    [UInt(32), Count] public uint Unknown0 { get; set; }
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown1 { get; } = [];
    [Array(CountFrom = nameof(Unknown0)), UInt(8)] public List<byte> Unknown2 { get; } = [];
    [UInt(32)] public uint Unknown3 { get; set; }
}
