using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(3)]
public partial class CombatLogDamage : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
    [UInt(1)] public bool Unknown7 { get; set; }
    [UInt(1)] public bool Unknown8 { get; set; }
    [UInt(3)] public byte Unknown9 { get; set; }
    [UInt(8)] public byte Unknown10 { get; set; }
    public CombatLogAction Unknown11 { get; set; } = new();
}
