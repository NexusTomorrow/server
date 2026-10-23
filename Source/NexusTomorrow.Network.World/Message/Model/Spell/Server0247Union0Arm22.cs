using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 34, which the client reads and drops.
/// </summary>
[WireStruct]
[UnionCase(34)]
public sealed partial class Server0247Union0Arm22 : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
