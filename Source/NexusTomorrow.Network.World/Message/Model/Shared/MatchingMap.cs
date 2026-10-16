using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Client05EFPart0
{
    [UInt(5)] public byte Unknown0 { get; private set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown1 { get; } = [];
    [UInt(14)] public ushort Unknown2 { get; private set; }
    [UInt(32)] public uint Unknown3 { get; private set; }
}
