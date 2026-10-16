using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client0144, Direction.ClientToServer)]
public sealed partial class Client0144
{
    [UInt(5)] public byte Unknown0 { get; private set; }
    [UInt(2)] public byte Unknown1 { get; private set; }
    [UInt(32), Count] public uint Unknown2 { get; private set; }
    [Array(CountFrom = nameof(Unknown2)), UInt(32)] public List<uint> Unknown3 { get; private set; } = [];
    [Array(CountFrom = nameof(Unknown2)), UInt(32)] public List<uint> Unknown4 { get; private set; } = [];
    [UInt(32), Count] public uint Unknown5 { get; private set; }
    [Array(CountFrom = nameof(Unknown5)), Float32] public List<float> Unknown6 { get; private set; } = [];
    [UInt(1)] public bool Unknown7 { get; private set; }
}
