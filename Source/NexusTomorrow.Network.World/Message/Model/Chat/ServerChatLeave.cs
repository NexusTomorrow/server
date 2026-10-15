using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatLeave)]
[WireMessage(GameMessageOpcode.ServerChatLeave, Direction.ServerToClient)]
public sealed partial class ServerChatLeave : IWritable
{
    public Channel Channel { get; set; }
    [UInt(2)] public ChatChannelLeaveReason Leave { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);
        writer.Write(Leave, 2u);
    }
}
