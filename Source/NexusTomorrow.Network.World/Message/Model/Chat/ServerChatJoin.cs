using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatJoin)]
public class ServerChatJoin : IWritable
{
    public Channel Channel { get; set; }
    public string Name { get; set; }
    public uint MemberCount { get; set; }
    public ChatChannelMemberFlags Flags { get; set; }
    public uint Order { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);
        writer.WriteStringWide(Name);
        writer.Write(MemberCount);
        writer.Write(Flags, 32u);
        writer.Write(Order);
    }
}
