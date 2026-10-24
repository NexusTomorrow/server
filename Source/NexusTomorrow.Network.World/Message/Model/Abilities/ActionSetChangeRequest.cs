using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Client00B1, Direction.ClientToServer)]
public sealed partial class ActionSetChangeRequest
{
    [WireStruct]
    public sealed partial class TierChange
    {
        [UInt(18)] public uint Unknown0 { get; private set; }
        [UInt(8)] public byte Unknown1 { get; private set; }
    }

    [Array(CountBits = 4), UInt(32)] public List<uint> Unknown0 { get; } = [];
    [UInt(3)] public byte Unknown1 { get; private set; }
    [Array(CountBits = 5)] public List<TierChange> Unknown2 { get; } = [];
    [Array(CountBits = 7), UInt(16)] public List<ushort> Unknown3 { get; } = [];
}
