using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(1)]
public sealed partial class CombatLogCCState : ICombatLogPayload
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }
    [UInt(14)] public ushort Unknown4 { get; set; }
    public CombatLogAction Unknown5 { get; set; } = new();
}
