using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Sets whether a player moderates a chat channel: the request a channel's SetModerator call sends, and its slash command.
/// </summary>
[WireMessage(GameMessageOpcode.Client01CD, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelSetModerator
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; private set; } = new();

    /// <summary>
    /// The player, by name.
    /// </summary>
    [WideString] public string PlayerName { get; private set; } = "";

    /// <summary>
    /// True to make the player a moderator, false to remove them.
    /// </summary>
    [UInt(1)] public bool IsModerator { get; private set; }
}
