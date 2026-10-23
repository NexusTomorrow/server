using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(29)]
public sealed partial class Server0247Union0Arm1D : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
}
