using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatNPCCustom)]
[WireMessage(GameMessageOpcode.ServerChatNPCCustom, Direction.ServerToClient)]
public sealed partial class ServerChatNPCCustom : IWritable
{
    public Channel Channel { get; set; }
    [UInt(21)] public uint UnitNameLocalizedTextId { get; set; }
    [WideString] public string CustomChatText { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);
        writer.Write(UnitNameLocalizedTextId, 21u);
        writer.WriteStringWide(CustomChatText);
    }
}
