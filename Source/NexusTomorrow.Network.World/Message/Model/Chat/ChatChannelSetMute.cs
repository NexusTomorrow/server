using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Mutes or unmutes a player in a chat channel: the request a channel's SetMute call sends, and its slash commands.
/// </summary>
[WireMessage(GameMessageOpcode.Client01CE, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelSetMute
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
    /// True to mute the player, false to unmute them.
    /// </summary>
    [UInt(1)] public bool IsMuted { get; private set; }
}
