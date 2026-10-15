using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
public sealed partial class LevelAuctionFilter : IAuctionFilter
{
    [UInt(32)] public uint Minimum { get; private set; }
    [UInt(32)] public uint Maximum { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Minimum = reader.ReadUInt();
        Maximum = reader.ReadUInt();
    }
}
