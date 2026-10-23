using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Asks for an operation on a guild: the request most GuildLib and GuildTypeLib calls send, each with its own operation.
/// </summary>
/// <remarks>
/// Each operation fills the rank, the value and the text it needs and leaves the others 0 or empty.
/// </remarks>
[WireMessage(GameMessageOpcode.Client04B1, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public partial class GuildOperationRequest
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new();

    /// <summary>
    /// A rank, from 0: the script call's rank less one, for the rank operations and the bank limits.
    /// </summary>
    [UInt(32)] public uint Rank { get; private set; }

    /// <summary>
    /// A value whose meaning depends on the operation: a perk's id, a bank limit, the bits of a rank's permissions or of the
    /// guild's flags, among others.
    /// </summary>
    [UInt(64)] public ulong Value { get; private set; }

    /// <summary>
    /// A member's name for the operations on a member, or a text: a message, a note or a name.
    /// </summary>
    [WideString] public string Text { get; private set; } = "";

    /// <summary>
    /// The operation, a GuildOperation value.
    /// </summary>
    [UInt(6)] public byte Operation { get; private set; }
}
