using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(23)]
public sealed partial class CombatLogUnit23 : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
