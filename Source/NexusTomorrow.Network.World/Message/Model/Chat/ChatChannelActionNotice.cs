using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Reports an action taken in a chat channel.
/// </summary>
/// <remarks>
/// The client raises ChatAction with the channel, the action, the actor and the target.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01B5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatChannelActionNotice
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; set; } = new();

    /// <summary>
    /// The player who acted, by name.
    /// </summary>
    [WideString] public string ActorName { get; set; } = "";

    /// <summary>
    /// The player acted on, by name.
    /// </summary>
    [WideString] public string TargetName { get; set; } = "";

    /// <summary>
    /// What happened, a ChatChannelAction value.
    /// </summary>
    [UInt(4)] public byte Action { get; set; }
}
