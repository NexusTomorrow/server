using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Activates one of a guild's perks.
/// </summary>
/// <remarks>
/// The client files the perk among the active ones and raises GuildPerkActivated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04B6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildPerkActivated
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The perk, by id.
    /// </summary>
    [UInt(14)] public ushort PerkId { get; set; }

    /// <summary>
    /// How many days the perk stays active.
    /// </summary>
    [Float32] public float DaysRemaining { get; set; }
}
