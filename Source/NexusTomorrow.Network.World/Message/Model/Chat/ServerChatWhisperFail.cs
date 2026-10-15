using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatWhisperFail)]
[WireMessage(GameMessageOpcode.ServerChatWhisperFail, Direction.ServerToClient)]
public sealed partial class ServerChatWhisperFail : IWritable
{
    [WideString] public string CharacterTo { get; set; }
    [UInt(1)] public bool IsAccountWhisper { get; set; }
    [UInt(16)] public ushort ChatMessageId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(CharacterTo);
        writer.Write(IsAccountWhisper);
        writer.Write(ChatMessageId);
    }
}
