using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatFlag)]
[WireMessage(GameMessageOpcode.ServerChatFlag, Direction.ServerToClient)]
public sealed partial class ServerChatFlag : IWritable
{
    public Channel ChatChannelId { get; set; }
    [UInt(64)] public ulong ChannelGuid { get; set; }
    [UInt(32)] public ChatChannelMemberFlags Flags { get; set; }

    public void Write(GamePacketWriter writer)
    {
        ChatChannelId.Write(writer);
        writer.Write(ChannelGuid);
        writer.Write(Flags, 32u);
    }
}
