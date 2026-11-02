using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Posts a line of the game's own text on a chat channel.
/// </summary>
[WireMessage(GameMessageOpcode.Server01C6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatNpcSay
{
    /// <summary>
    /// The channel.
    /// </summary>
    [VerifiedMember] public ChatChannelKey Channel { get; set; } = new();

    /// <summary>
    /// The line's primary text, as a text id.
    /// </summary>
    [VerifiedMember, UInt(21)] public uint TextId { get; set; }

    /// <summary>
    /// The line's secondary text, as a text id; when 0 the client posts nothing.
    /// </summary>
    [VerifiedMember, UInt(21)] public uint SecondaryTextId { get; set; }
}
