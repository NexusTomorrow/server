using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server071A, Direction.ServerToClient)]
public sealed partial class RaidLockouts
{
    [WireStruct]
    public sealed partial class RaidLockout
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(15)] public ushort Unknown1 { get; set; }
        [UInt(64)] public ulong Unknown2 { get; set; }
        [Float32] public float Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
    }

    [Array(CountBits = 32)] public List<RaidLockout> Unknown0 { get; set; } = [];
}
