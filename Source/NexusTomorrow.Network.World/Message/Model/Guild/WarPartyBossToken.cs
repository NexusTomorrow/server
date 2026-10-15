using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

public class WarPartyBossToken : IWritable
{
    public uint TokenItem2Id { get; set; }
    public uint Count { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TokenItem2Id, 18u);
        writer.Write(Count);
    }
}
