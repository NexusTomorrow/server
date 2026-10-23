using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(12)]
public sealed partial class CombatLogHeal : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(8)] public byte Unknown3 { get; set; }
    public CombatLogAction Unknown4 { get; set; } = new();
}
