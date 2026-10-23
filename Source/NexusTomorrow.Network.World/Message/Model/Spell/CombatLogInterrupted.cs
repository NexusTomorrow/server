using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(19)]
public sealed partial class CombatLogInterrupted : ICombatLogPayload
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(9)] public ushort Unknown1 { get; set; }
    public CombatLogAction Unknown2 { get; set; } = new();
}
