using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChat, Direction.ClientToServer)]
public sealed partial class ClientChat
{
    public Channel Channel { get; private set; }
    [WideString] public string Message { get; private set; }
    [Array(CountBits = 5)] public List<ChatClientFormat> Formats { get; } = [];
    [UInt(16)] public ushort ChatMessageId { get; private set; } // incremented by the client
}
