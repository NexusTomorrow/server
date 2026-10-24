using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server049F, Direction.ServerToClient)]
public sealed partial class Server049F
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The member, by character id.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    [UInt(32)] public uint Unknown2 { get; set; }
}
