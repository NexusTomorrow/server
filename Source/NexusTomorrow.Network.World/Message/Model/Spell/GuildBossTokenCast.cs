using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Casts a guild boss token.
/// </summary>
/// <remarks>
/// The client starts the cast itself; the server answers by its id with <see cref="SpellConfirmCast"/> or
/// <see cref="SpellCastResult"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client094F, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class GuildBossTokenCast
{
    /// <summary>
    /// The guild, by its id within the realm.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new RealmScopedId();

    [UInt(18)] public uint Unknown1 { get; private set; }

    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; private set; }
}
