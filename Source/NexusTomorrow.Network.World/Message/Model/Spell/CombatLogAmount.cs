using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(0)]
public sealed partial class CombatLogAmount : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public CombatLogAction Unknown1 { get; set; } = new();
}
