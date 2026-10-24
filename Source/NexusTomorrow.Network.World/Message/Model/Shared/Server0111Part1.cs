using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0111Part1
{
    [WireStruct]
    public sealed partial class Server0111Part3
    {
        [UInt(3)] public byte Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server0111Part4
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown1 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(18)] public uint Unknown2 { get; set; }
    public ItemLocation Unknown3 { get; set; } = new();
    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// The item's charges, which Item:GetCharges reports.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Charges { get; set; }

    [UInt(64)] public ulong Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(64)] public ulong Unknown8 { get; set; }
    [Float32] public float Unknown9 { get; set; }
    [UInt(32)] public uint Unknown10 { get; set; }
    [UInt(8)] public byte Unknown11 { get; set; }
    [UInt(32)] public uint Unknown12 { get; set; }
    [UInt(32)] public uint Unknown13 { get; set; }
    [UInt(32)] public uint Unknown14 { get; set; }
    [FixedArray(2)] public Server0111Part3[] Unknown15 { get; set; } = [new(), new()];

    /// <summary>
    /// The power core applied to the item, as an id in the client's Item2 table. When that item is a power core, the client applies its
    /// level range to the stats it works out for this item.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint PowerCoreItem2Id { get; set; }

    [Array(CountBits = 3), UInt(32)] public List<uint> Unknown17 { get; } = [];
    [Array(CountBits = 4), UInt(32)] public List<uint> Unknown18 { get; } = [];
    [Array(CountBits = 6)] public List<Server0111Part4> Unknown19 { get; } = [];
    [UInt(32)] public uint Unknown20 { get; set; }
}
