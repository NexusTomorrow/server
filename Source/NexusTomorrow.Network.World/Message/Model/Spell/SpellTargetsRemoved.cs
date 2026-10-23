using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Removes targets from a cast.
/// </summary>
/// <remarks>
/// With no targets left, the client raises SprintStateUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0811, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellTargetsRemoved
{
    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }

    /// <summary>
    /// The targets removed, by unit id.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> TargetUnitIds { get; set; } = [];
}
