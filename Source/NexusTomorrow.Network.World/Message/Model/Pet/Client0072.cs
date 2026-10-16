using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Client0072, Direction.ClientToServer)]
public sealed partial class Client0072
{
    [UInt(32), Count] public uint Unknown0 { get; set; }
    [Array(CountFrom = nameof(Unknown0)), UInt(32)] public List<uint> Unknown1 { get; } = [];
    [Array(CountFrom = nameof(Unknown0)), UInt(8)] public List<byte> Unknown2 { get; } = [];
}
