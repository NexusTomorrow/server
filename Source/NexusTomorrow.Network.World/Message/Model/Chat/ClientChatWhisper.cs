using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChatWhisper, Direction.ClientToServer)]
public sealed partial class ClientChatWhisper
{
    [WideString] public string ToName { get; private set; }
    [WideString] public string ToRealmName { get; private set; }

    [WideString] public string Message { get; private set; }
    [Array(CountBits = 5)] public List<ChatClientFormat> Formats { get; } = [];
    [UInt(16)] public ushort ChatMessageId { get; set; } // incremented by the client

    [UInt(1)] public bool IsAccountWhisper { get; set; }
}
