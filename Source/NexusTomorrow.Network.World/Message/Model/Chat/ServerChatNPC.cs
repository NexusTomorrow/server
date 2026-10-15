using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatNPC)]
[WireMessage(GameMessageOpcode.ServerChatNPC, Direction.ServerToClient)]
public sealed partial class ServerChatNPC : IWritable
{
    public Channel Channel { get; set; }
    [UInt(21)] public uint UnitNameLocalizedTextId { get; set; }
    [UInt(21)] public uint MessageLocalizedTextId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);
        writer.Write(UnitNameLocalizedTextId, 21u);
        writer.Write(MessageLocalizedTextId, 21);
    }
}
