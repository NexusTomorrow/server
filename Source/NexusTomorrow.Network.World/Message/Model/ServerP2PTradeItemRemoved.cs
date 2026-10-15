using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerP2PTradeItemRemoved)]
[WireMessage(GameMessageOpcode.ServerP2PTradeItemRemoved, Direction.ServerToClient)]
public sealed partial class ServerPTPTradeItemRemoved : IWritable
{
    [UInt(64)] public ulong ItemGuid { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ItemGuid);
    }
}
