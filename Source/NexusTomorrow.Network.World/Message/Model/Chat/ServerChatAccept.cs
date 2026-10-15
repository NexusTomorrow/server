using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Chat.Model;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

// Sent back to the client after the client sends any chat message. May send item details for the chat message.
// If there was an item linked, the client used this data to go through the previous message specifed by
// ChatMesssageId and replaces any Formats of type ItemGuid with ItemFull.

[WireMessage(GameMessageOpcode.ServerChatAccept, Direction.ServerToClient)]
public sealed partial class ServerChatAccept
{
    [UInt(16)] public ushort ChatMessageId { get; set; } // used to match the original ClientChat message
    [UInt(1)] public bool GM { get; set; }
    [Array(CountBits = 5)] public List<ChatFormatItemFull> Formats { get; } = [];
    [WideString] public string SenderName { get; set; }
    [WideString] public string RealmName { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(8)] public byte PremiumTier { get; set; } = 1;
}
