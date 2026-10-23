using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Finishes a cast.
/// </summary>
/// <remarks>
/// The client raises SprintStateUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server07FE, Direction.ServerToClient)]
[Verified(Rung.Fields)]

public sealed partial class SpellFinish
{
    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }
}
