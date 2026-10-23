using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Casts a spell paid for with service tokens.
/// </summary>
/// <remarks>
/// The client refuses it when the player has too few tokens.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00C2, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ServiceTokenCast
{
    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; private set; }

    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; private set; }
}
