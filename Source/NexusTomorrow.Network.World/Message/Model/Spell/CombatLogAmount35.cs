using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(35)]
public sealed partial class CombatLogAmount35 : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
