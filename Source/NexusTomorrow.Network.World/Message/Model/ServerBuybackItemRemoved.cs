using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerBuybackItemRemoved)]
[WireMessage(GameMessageOpcode.ServerBuybackItemRemoved, Direction.ServerToClient)]
public sealed partial class ServerBuybackItemRemoved : IWritable
{
    [UInt(32)] public uint UniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UniqueId);
    }
}
