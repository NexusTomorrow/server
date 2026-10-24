using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sends a value for a guild.
/// </summary>
[WireMessage(GameMessageOpcode.Server0480, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildClassificationChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    [UInt(3)] public byte Unknown1 { get; set; }
}
