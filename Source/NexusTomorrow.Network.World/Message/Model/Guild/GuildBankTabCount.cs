using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets how many bank tabs a guild has.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetBankTabCount returns it; the client raises GuildBankTabCount when it changes.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0478, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildBankTabCount
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// How many bank tabs the guild has.
    /// </summary>
    [UInt(32)] public uint TabCount { get; set; }
}
