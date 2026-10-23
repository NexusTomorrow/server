using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Reports the result of sending a chat line.
/// </summary>
/// <remarks>
/// The client raises ChatResult, unless the channel's ChatChannel row suppresses it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01D3, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatSendResult
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; set; } = new();

    /// <summary>
    /// The result, a ChatResult value.
    /// </summary>
    [UInt(5)] public byte Result { get; set; }

    /// <summary>
    /// The line the result is for, by the slot the client filed it under when it sent it; the client frees the slot.
    /// </summary>
    [UInt(16)] public ushort ChatId { get; set; }
}
