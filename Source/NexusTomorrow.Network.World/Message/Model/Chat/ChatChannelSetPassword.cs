using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Sets a chat channel's password: the request a channel's SetPassword call sends, and its slash command.
/// </summary>
[WireMessage(GameMessageOpcode.Client01D0, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelSetPassword
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; private set; } = new();

    /// <summary>
    /// The new password; the slash command sends it empty when given none.
    /// </summary>
    [WideString] public string Password { get; private set; } = "";
}
