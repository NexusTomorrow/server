using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Lists a chat channel's members: the answer to <see cref="ChatChannelRequestMembers"/>.
/// </summary>
/// <remarks>
/// The client raises ChatList.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01C0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelMembers
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; set; } = new();

    /// <summary>
    /// How many members the two lists hold.
    /// </summary>
    [UInt(32), Count] public uint MemberCount { get; set; }

    /// <summary>
    /// The members, by name.
    /// </summary>
    [Array(CountFrom = nameof(MemberCount)), WideString] public List<string> Names { get; set; } = [];

    /// <summary>
    /// Each member's flags in the same order, a ChatChannelMemberFlags value: bit 0 owner, bit 1 moderator, bit 2 muted.
    /// </summary>
    [Array(CountFrom = nameof(MemberCount)), UInt(32)] public List<uint> Flags { get; set; } = [];

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(1)] public bool Unknown4 { get; set; }
}
