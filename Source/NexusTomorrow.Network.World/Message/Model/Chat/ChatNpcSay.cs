using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Posts a line of the game's own text on a chat channel.
/// </summary>
[WireMessage(GameMessageOpcode.Server01C6, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ChatNpcSay
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; set; } = new();

    [UInt(21)] public uint Unknown1 { get; set; }

    [UInt(21)] public uint Unknown2 { get; set; }
}
