using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part8
{
    /// <summary>
    /// The target, by unit id. The client attaches the target's effects to that unit.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    [UInt(8)] public byte Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }

    /// <summary>
    /// The target's stack count, which Unit:GetBuffs reports as nCount. The client also sums it over a unit's targets.
    /// </summary>
    [VerifiedMember, UInt(16)] public ushort StackCount { get; set; }

    [UInt(4)] public byte Unknown4 { get; set; }

    /// <summary>
    /// The effects on the target.
    /// </summary>
    [VerifiedMember, Array(CountBits = 8)] public List<Server0262Part9> Effects { get; set; } = [];
}
