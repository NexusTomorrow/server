using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatNPC)]
public class ServerChatNPC : IWritable
{
    public Channel Channel { get; set; }
    public uint UnitNameLocalizedTextId { get; set; }
    public uint MessageLocalizedTextId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);
        writer.Write(UnitNameLocalizedTextId, 21u);
        writer.Write(MessageLocalizedTextId, 21);
    }
}
