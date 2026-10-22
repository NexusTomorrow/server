using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a guild's flags.
/// </summary>
/// <remarks>
/// The client then raises GuildFlags.
/// </remarks>
[WireMessage(GameMessageOpcode.Server048B, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildFlags
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's flags, stored whole.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
