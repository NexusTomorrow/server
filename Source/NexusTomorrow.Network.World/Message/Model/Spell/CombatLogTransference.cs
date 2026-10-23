using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(15)]
public sealed partial class CombatLogTransference : ICombatLogPayload
{
    [WireStruct]
    public sealed partial class CombatLogTransferenceHeal
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(5)] public byte Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
    [Array(CountBits = 32)] public List<CombatLogTransferenceHeal> Unknown7 { get; set; } = [];
}
