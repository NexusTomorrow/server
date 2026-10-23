using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Posts a line on a chat channel, part from the game's own text and part given.
/// </summary>
[WireMessage(GameMessageOpcode.Server01CA, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ChatNpcSayCustom
{
    /// <summary>
    /// The channel.
    /// </summary>
    public ChatChannelKey Channel { get; set; } = new();

    [UInt(21)] public uint Unknown1 { get; set; }

    [WideString] public string Unknown2 { get; set; } = "";
}
