using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Reports the result of joining a chat channel.
/// </summary>
/// <remarks>
/// The client raises ChatJoinResult with the name and the result.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01BB, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatJoinChannelResult
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(14)] public ushort Unknown0 { get; set; }

    /// <summary>
    /// The channel, by name.
    /// </summary>
    [WideString] public string ChannelName { get; set; } = "";

    /// <summary>
    /// The result, which the client passes to ChatJoinResult.
    /// </summary>
    [UInt(5)] public byte Result { get; set; }
}
