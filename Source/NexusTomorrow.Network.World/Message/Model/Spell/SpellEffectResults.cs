using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Reports the results of a cast's effects on a target.
/// </summary>
[WireMessage(GameMessageOpcode.Server07F8, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellEffectResults
{
    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }

    [UInt(19)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }

    [Array(CountBits = 8)] public List<Server0262Part10> Unknown3 { get; set; } = [];
}
