using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Reports why a cast the client started failed.
/// </summary>
/// <remarks>
/// The client reports the failure as it does its own refusals, and raises SprintStateUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server07FC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellCastResult
{
    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; set; }

    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; set; }

    /// <summary>
    /// The result, a Spell4CastResult table id.
    /// </summary>
    [UInt(9)] public ushort Result { get; set; }
}
