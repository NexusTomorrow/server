using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
public sealed partial class PropertyMaxAuctionFilter : IAuctionFilter
{
    [UInt(8)] public Property UnitProperty2Id { get; private set; }
    [Float32] public float Maximum { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UnitProperty2Id = reader.ReadEnum<Property>(8u);
        Maximum = reader.ReadSingle();
    }
}
