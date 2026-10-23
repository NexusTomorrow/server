using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01BF, Direction.ServerToClient)]
public sealed partial class ChatChannelLeft
{
    public ChatChannelKey Unknown0 { get; set; } = new();
    [UInt(2)] public byte Unknown1 { get; set; }
}
