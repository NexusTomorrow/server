using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class WarPartyBossToken : IWritable
{
    [UInt(18)] public uint TokenItem2Id { get; set; }
    [UInt(32)] public uint Count { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TokenItem2Id, 18u);
        writer.Write(Count);
    }
}
