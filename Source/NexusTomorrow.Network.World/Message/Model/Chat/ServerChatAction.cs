using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatAction)]
[WireMessage(GameMessageOpcode.ServerChatAction, Direction.ServerToClient)]
public sealed partial class ServerChatAction : IWritable
{
    public Channel Channel { get; set; }
    [WideString] public string NameActor { get; set; }
    [WideString] public string NameActedOn { get; set; }
    [UInt(4)] public ChatChannelAction Action { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);
        writer.WriteStringWide(NameActor);
        writer.WriteStringWide(NameActedOn);
        writer.Write(Action, 4u);
    }
}
