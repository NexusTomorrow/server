using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(22)]
public sealed partial class CombatLogUnit : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
